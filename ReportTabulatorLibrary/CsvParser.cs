using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReportTabulatorLibrary
{
    public class CsvParser
    {
        private readonly string _file;
        private readonly string _separator;

        public CsvParser(string file, string separator)
        {
            _file = file;
            _separator = separator;
        }

        private List<string> GetLines()
        {
            if (string.IsNullOrWhiteSpace(_separator)) return null;
            if (string.IsNullOrWhiteSpace(_file)) return null;

            try
            {
                return File.ReadAllLines(_file).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<FaxTabEntry> GetTabEntries(IEnumerable<string> lines)
        {
            /*
                0* Mail Message Id
                1* |Workflow Id
                2* |Org Id
                3* |Org
                4* |Zip
                5* |Address
                6* |Unit
                7* |City
                8* |State
                9* |Reseller
                10* |Reseller Url
                11* |Hsa Location
                12* |Hrr Location
                13* |Release Type
                14* |Sub Domain
                15* |Source Type
                16* |Status
                17* |Date
                18 * |To
                19* |From
                20* |Origin
                21* |Fax Page Count
                22* |Original Fax Page Count
            */
            try
            {
                return 
                    lines.Skip(1).Select(line => line.Split(_separator[0])).Select(values => new FaxTabEntry
                    {
                        Date = DateTime.Parse(values[17]),
                        FromAddress = values[19],
                        MailMessageId = long.Parse(values[0]),
                        OrganizationId = long.Parse(values[2]),
                        OrganizationName = values[3],
                        Origin = values[20],
                        ToAddress = values[18],
                        OriginalPageCount = int.Parse(values[22]),
                        Status = values[16],
                        WorkflowId = Guid.Parse(values[1])
                    }).ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        private List<FaxTabEntry> GetOnlyFaxEntries(List<FaxTabEntry> entries)
        {
            return entries.Where(x =>
                !string.IsNullOrWhiteSpace(x.ToAddress)
                && !string.IsNullOrWhiteSpace(x.FromAddress)
                && x.ToAddress.IsOnlyDigits()).ToList();
        }

        public string Process()
        {

            var lines = GetLines();
            if (lines == null) return "Unable to process file.";
            var tabEntries = GetTabEntries(lines);
            if (tabEntries == null) return "Incompatible file content format detected.";

            var faxesOnly = GetOnlyFaxEntries(tabEntries);

            var orgPageCounts = faxesOnly.GroupBy(x => x.OrganizationId)
                .Select(
                    org =>
                        new {
                            Organization = org.First().OrganizationName,
                            PagesSent = org.Where(x => x.Origin == "Kno2WebPortal").Sum(x => x.OriginalPageCount),
                            PagesReceived = org.Where(x => x.Origin == "Fax").Sum(x => x.OriginalPageCount)
                        }).ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"Results: {tabEntries.Count}");
            sb.AppendLine($"Faxes sent or received: {faxesOnly.Count}");
            sb.AppendLine(
                $"Total pages sent or received: {faxesOnly.Sum(f => f.OriginalPageCount)}");
            foreach (var org in orgPageCounts)
            {
                sb.AppendLine($"{org.Organization} - Pages Sent: {org.PagesSent} - Pages Received: {org.PagesReceived}");
            }
            return sb.ToString();
        }
    }
}
