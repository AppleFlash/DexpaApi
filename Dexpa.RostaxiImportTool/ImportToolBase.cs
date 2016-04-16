using System;
using System.Data.Entity;
using System.Threading;

namespace Dexpa.RostaxiImportTool
{
    abstract class ImportToolBase
    {
        public int RecordsImported { get; protected set; }

        protected string mFileName;

        protected DbContext mDbContext;

        public bool Completed { get; private set; }

        public Exception Error { get; private set; }

        public ImportToolBase(string filename, DbContext context)
        {
            mFileName = filename;
            mDbContext = context;
        }

        public void StartImport()
        {
            Completed = false;
            var thread = new Thread(() =>
            {
                try
                {
                    ImportData();
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
                Completed = true;
            });
            thread.IsBackground = true;
            thread.Start();
        }
        
        protected abstract void ImportData();
    }
}
