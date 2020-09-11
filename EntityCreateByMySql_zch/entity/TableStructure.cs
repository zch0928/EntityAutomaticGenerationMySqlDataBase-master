namespace EntityCreateByMySql_zch.entity
{
    class TableStructure
    {
        private string Field;
        private string Type;

        public TableStructure()
        {
        }

        public TableStructure(string field, string type)
        {
            Field = field;
            Type = type;
        }

        public string Field1 { get => Field; set => Field = value; }
        public string Type1 { get => Type; set => Type = value; }
    }
}
