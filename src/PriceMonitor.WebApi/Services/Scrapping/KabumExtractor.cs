﻿namespace PriceMonitor.WebApi.Services.Scrapping
{
    [WebsiteAttribute("kabum")]
    public class KabumExtractor : BaseExtractor, IExtractor
    {
        private readonly string[] InCashValueTag = new string[] { ".preco_desconto_avista-cm", ".preco_desconto" };
        private readonly string[] NormalValueTag = new string[] { ".preco_desconto-cm", ".preco_normal" };
        private readonly string[] FullValueTag = new string[] { ".preco_antigo-cm", ".preco_antigo" };
        private readonly string AvailabilityTag = ".produto_indisponivel";
        private readonly string TitleTag = ".titulo_det";

        protected override void FillValues()
        {
            InCashValue = ExtractValueFromTags(InCashValueTag);
            NormalValue = ExtractValueFromTags(NormalValueTag);
            FullValue = ExtractValueFromTags(FullValueTag);
        }

        protected override void CheckAvailability()
        {
            IsAvailable = Document.QuerySelector(AvailabilityTag) == null;                
        }

        protected override bool ConfirmIfLoaded()
        {
            return Document.QuerySelector(TitleTag) != null;
        }
    }
}
