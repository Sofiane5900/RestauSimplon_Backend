namespace RestauSimplon.Models
{
    public class CommandeArticle
    {
        public int CommandeId { get; set; }
        public Commande Commande { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }

}
