using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace AssinaturaDigital
{
    public abstract class Assinador<T>
    {
        private readonly string SenhaCertificado;
        private readonly byte[] Certificado;
        protected X509Certificate2 X509Certificado2 { get; private set; }

        public Assinador(byte[] certificado, string senhaCertificado)
        {
            Certificado = certificado;
            SenhaCertificado = senhaCertificado;
            X509Certificado2 = new X509Certificate2(Certificado, SenhaCertificado);
        }

        public abstract T Assinar();
    }
}
