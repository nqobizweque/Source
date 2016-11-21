using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySchedule.ViewModels
{
    public class ReportViewModel
    {
        public int totalEvents { get; set; }
        public int totalUpcomingEvents { get; set; }       

        public int totalLocations { get; set; }
        public int freqLocationCount { get; set; }
        public string mostFreqLocation { get; set; }
        public int totalLocationlessEvents { get; set; }

        public int totalCategories { get; set; }
        public int freqCategoryCount { get; set; }
        public string mostFreqCategory { get; set; }
        public int totalCategorylessEvents { get; set; }    

        public int totalModules { get; set; }
        public int freqModuleCount { get; set; }
        public string mostFreqModule { get; set; }
        public int totalModulessEvents { get; set; }

        public int totalTasks { get; set; }
        public int totalUpcomingTasks { get; set; }

    }
}