using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Security;
using Syncfusion.Compression;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace AssinaturaDigital
{
    public class AssinadorPdf : Assinador<PdfDocument>
    {
        private readonly Stream ImagemAssinatura;
        protected Stream ArquivoParaAssinar { get; private set; }
        private readonly string InformacaoContato;
        private readonly string Local;
        private readonly string Razao;

        public AssinadorPdf(byte[] certificado, string senhaCertificado, Stream arquivoParaAssinar, string informacaoContato, string local, string razao, Stream imagemAssinatura) : base(certificado, senhaCertificado)
        {
            ImagemAssinatura = imagemAssinatura;
            ArquivoParaAssinar = arquivoParaAssinar;
            InformacaoContato = informacaoContato;
            Local = local;
            Razao = razao;
        }

        public override PdfDocument Assinar()
        {
            PdfDocument documetno = new PdfDocument();
            documetno.Append(new Syncfusion.Pdf.Parsing.PdfLoadedDocument(ArquivoParaAssinar, true));

            PdfGraphics graphics = documetno.Pages[documetno.Pages.Count - 1].Graphics;

            PdfCertificate pdfCert = new PdfCertificate(X509Certificado2);
            PdfSignature signature = new PdfSignature(documetno, documetno.Pages[documetno.Pages.Count - 1], pdfCert, "Assinatura");

            PdfBitmap signatureImage = new PdfBitmap(ImagemAssinatura);
            signature.Bounds = new RectangleF(0, 0, 200, 100);
            signature.ContactInfo = InformacaoContato;
            signature.LocationInfo = Local;
            signature.Reason = Razao;
            graphics.DrawImage(signatureImage, signature.Bounds);
            return documetno;
        }
    }
}