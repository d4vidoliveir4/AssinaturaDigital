﻿using System.Security.Cryptography.Xml;
using System.Xml;

namespace AssinaturaDigital
{
    public class AssinadorXml : Assinador<XmlDocument>
    {
        public AssinadorXml(byte[] certificado, string senhaCertificado, string xml) : base(certificado, senhaCertificado, xml)
        {
        }

        public override XmlDocument Assinar()
        {
            var documento = new XmlDocument();
            documento.LoadXml(Xml);
            var docXml = new SignedXml(documento) { SigningKey = X509Certificado2.PrivateKey };

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
            keyInfo.AddClause(new KeyInfoX509Data(X509Certificado2));

            docXml.KeyInfo = keyInfo;
            docXml.ComputeSignature();

            XmlElement xmlDigitalSignature = docXml.GetXml();

            documento.DocumentElement.AppendChild(documento.ImportNode(xmlDigitalSignature, true));

            return documento;
        }

    }

}
