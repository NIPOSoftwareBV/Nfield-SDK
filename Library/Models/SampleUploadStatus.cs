using System;

namespace Nfield.Models
{
    public class SampleUploadStatus
    {
        public string ProcessingStatus { get; set; }
        public int TotalRecordCount { get; set; }
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int DuplicateKeyCount { get; set; }
        public int EmptyKeyCount { get; set; }
        public int InvalidKeyCount { get; set; }
        public int InvalidDataCount { get; set; }
        public int SkippedCount { get; set; }
        public bool HeaderInvalid { get; set; }
        public int HeaderInvalidColumnsCount { get; set; }
        public bool HeaderDataMismatch { get; set; }
    }
}
