namespace UtilityDomain
{
    public class BoolResult : CustomErrorObject
    {
        public BoolResult(bool okresult, string errorMessage)
        {
            OkResult = okresult;
            ErrorMessage = errorMessage;
        }

        public BoolResult(bool okResult)
        {
            OkResult = okResult;
            ErrorMessage = "";
        }

        public BoolResult()
        {

        }

        public bool OkResult { get; set; }
    }
}