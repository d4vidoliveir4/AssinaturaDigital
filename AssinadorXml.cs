using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace AssinaturaDigital
{
    public class AssinadorXml
    {
        private readonly string SenhaCertificado;
        private readonly byte[] Certificado;
        private readonly string Xml;

        public AssinadorXml(byte[] certificado, string senhaCertificado, string xml)
        {
            this.Certificado = certificado; 
            this.SenhaCertificado = senhaCertificado;
            this.Xml = xml;
        }

        private X509Certificate2 LerCertificado()
        {
            return new X509Certificate2(Certificado,SenhaCertificado);
        }

        public XmlDocument Assinar()
        {
            var certificado = LerCertificado();
            
            var documento = new XmlDocument();
            documento.LoadXml(Xml);
            var docXml = new SignedXml(documento) { SigningKey = certificado.PrivateKey };

            Reference referencia = new Reference
            {
                Uri = ""
            };

            var envelope = new XmlDsigEnvelopedSignatureTransform();
            referencia.AddTransform(envelope);

            var c14Transform = new XmlDsigC14NTransform();
            referencia.AddTransform(c14Transform);

            docXml.AddReference(referencia);

            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));

            docXml.KeyInfo = keyInfo;
            docXml.ComputeSignature();

            XmlElement xmlDigitalSignature = docXml.GetXml();

            documento.DocumentElement.AppendChild(documento.ImportNode(xmlDigitalSignature, true));

            return documento;
        }

    }

}
