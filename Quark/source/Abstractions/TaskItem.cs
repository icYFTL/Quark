using System.Collections.Generic;


namespace Quark.source.Abstractions
{
    class TaskItem
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private List<object> _content;
        public List<object> Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public TaskItem(string Name, string Description, List<object> Content)
        {
            this.Name = Name;
            this.Description = Description;
            this.Content = Content;
        }
    }
}
