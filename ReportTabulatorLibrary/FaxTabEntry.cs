using System;

namespace ReportTabulatorLibrary
{
    public class FaxTabEntry
    {
        //Mail Message Id|Workflow Id|Org Id|Org|Zip|Address|Unit|City|State|Reseller|Reseller Url|Hsa Location|Hrr Location|Release Type|Sub Domain|Source Type|Status|Date|To|From|Origin|Fax Page Count|Original Fax Page Count
        public long MailMessageId { get; set; }
        public Guid WorkflowId { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime Date { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public string Origin { get; set; }
        public string Status { get; set; }
        public int OriginalPageCount { get; set; }
    }
}