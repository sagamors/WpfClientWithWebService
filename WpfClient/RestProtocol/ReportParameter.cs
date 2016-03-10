namespace RestProtocol
{
    public abstract class ReportParameter
    {

        protected ReportParameter(string name, int code)
        {
            Name = name;
            Code = code;
        }

        public int Code { private set; get; }
        public string Name { private set; get; }
    }
}