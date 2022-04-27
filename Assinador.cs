using System.Security.Cryptography.X509Certificates;

namespace AssinaturaDigital
{
    public abstract class Assinador<T>
    {
        private readonly string SenhaCertificado;
        private readonly byte[] Certificado;
        protected string Xml { get; private set; }
        protected X509Certificate2 X509Certificado2 { get; private set; }

        public Assinador(byte[] certificado, string senhaCertificado, string xml)
        {
            Certificado = certificado;
            SenhaCertificado = senhaCertificado;
            Xml = xml;
            X509Certificado2 = new X509Certificate2(Certificado, SenhaCertificado);
        }

        public abstract T Assinar();
    }
}
