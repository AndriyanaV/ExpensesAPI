namespace ExpensesAPI.Dto.ExpenceDto
{
    public class ExpenseForCreationDto
    {
        public string Title { get; set; }

        public int Amount { get; set; }

        public string Description { get; set; }

        public int categoryId { get; set; }

        public int paymentMethodId {  get; set; }
    }
}
