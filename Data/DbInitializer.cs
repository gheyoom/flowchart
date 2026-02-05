using Microsoft.EntityFrameworkCore;
using FlowChartApp.Models;
using System.Text.Json;

namespace FlowChartApp.Data;

public static class DbInitializer
{
    public async static Task Initialize(AppDbContext context)
    {
            // Clear existing
            context.FlowConnections.RemoveRange(context.FlowConnections);
            context.FlowBoxes.RemoveRange(context.FlowBoxes);
            context.LegendItems.RemoveRange(context.LegendItems);
            context.AppMetadata.RemoveRange(context.AppMetadata);
            context.SaveChanges();
            context.SaveChanges();

            var boxes = new List<FlowBox>();
            var connections = new List<FlowConnection>();

            FlowBox CreateBox(string name, string? desc, string? descAr, double x, double y, string color, string width = "220px", string height = "auto")
            {
                return new FlowBox
                {
                    Name = name,
                    Description = desc,
                    DescriptionArabic = descAr,
                    PosX = x,
                    PosY = y,
                    BackgroundColor = color,
                    BorderColor = "#000000",
                    Width = width,
                    Height = height
                };
            }
            
            // Helper for Connections
            void Connect(FlowBox s, FlowBox t, string dir = "bottom") {
                connections.Add(new FlowConnection { SourceBox = s, TargetBox = t, Direction = dir, ConnectionType = "solid", Color = "#000000" });
            }

            // Colors
            string cYellow = "#FFFF00"; 
            string cGreen = "#90EE90"; 
            string cBlue = "#6495ED"; 
            string cPink = "#FFB6C1"; 
            string cOrange = "#F4B084"; 
            string cWhite = "#FFFFFF";

            // Root
            var root = CreateBox("NDP 01", "General Naval Doctrine", "عقيدة القوات البحرية", 1500, 50, cWhite, "300px", "150px");
            boxes.Add(root);

            double row1Y = 300;
            // Rows for children - Increased spacing to avoid overlaps
            // If Row 1 boxes are ~200px tall (ends at 500), Row 2 should start at 600+.
            double row2Y = 650;
            double row3Y = 950;
            double row4Y = 1250;
            double row5Y = 1550;
            double row6Y = 1850;

            // ================= L1 Nodes =================
            // 1. NDP 1 (Yellow)
            var ndp1 = CreateBox("NDP 1", "Human Resources Doctrine", "عقيدة القوى البشرية", 100, row1Y, cYellow);
            boxes.Add(ndp1);
            Connect(root, ndp1);

            // 2. Intelligence (Yellow)
            var intel = CreateBox("INTELLIGENCE", null, "الاستخبارات", 350, row1Y, cYellow);
            boxes.Add(intel);
            Connect(root, intel);

            // 3. NDP 2 Group (Blue Stack)
            // Stacking 2.1 under the main line manually since it's an L1 peer visually in placement but logical child
            var ndp21 = CreateBox("NDP 2.1", "Warfare", "الأسطول", 600, row1Y + 50, cBlue); 
            boxes.Add(ndp21);
            Connect(root, ndp21); 

            var ndp22 = CreateBox("NDP 2.2", "Maritime Security", "الأمن البحري", 600, row1Y + 250, cBlue);
            boxes.Add(ndp22);
            Connect(ndp21, ndp22); // Stack 2.1 -> 2.2

            var ndp221 = CreateBox("NDP 2.2.1", "Law of Sea", "قانون البحار", 600, row3Y - 50, cPink);
            boxes.Add(ndp221);
            Connect(ndp22, ndp221);

            // 4. NDP 3 (Green) - Central Hub
            // Reduced height from 250px to 200px to avoid hitting next row
            var ndp3 = CreateBox("NDP 3", "FLEET NS", "(NTP 01)", 1300, row1Y, cGreen, "250px", "180px");
            boxes.Add(ndp3);
            Connect(root, ndp3);

            // 5. NDP 4 (Yellow)
            var ndp4 = CreateBox("NDP 4", "LOGISTICS", "اللوجستيات", 2200, row1Y, cYellow);
            boxes.Add(ndp4);
            Connect(root, ndp4);

            // 6. NDP 5 (Green)
            var ndp5 = CreateBox("NDP 5", "Planning Doctrine", "عقيدة التخطيط", 2500, row1Y, cGreen);
            boxes.Add(ndp5);
            Connect(root, ndp5);

            // 7. NDP 6 (Yellow)
            var ndp6 = CreateBox("NDP 6", "COMMUNICATIONS & CIS", "والاتصالات", 2800, row1Y, cYellow);
            boxes.Add(ndp6);
            Connect(root, ndp6);

            // 8. NDP 7 (Pink)
            var ndp7 = CreateBox("NDP 7", "TRAINING", "التدريب", 3100, row1Y, cPink);
            boxes.Add(ndp7);
            Connect(root, ndp7);

            // 9. NDP 8 (White)
            var ndp8 = CreateBox("NDP 8", "Rules", "القواعد", 3400, row1Y, cWhite);
            boxes.Add(ndp8);
            Connect(root, ndp8);


            // ================= NDP 3 Children (Horizontal Row) =================
            // Spanning from ~800 to ~2000
            double ndp3Start = 850;
            double spacing = 220;

            // 3.1 Pink
            var ndp31 = CreateBox("NDP 3.1", "Surface Warfare", "حرب سطحية", ndp3Start, row2Y, cPink, "200px");
            boxes.Add(ndp31); Connect(ndp3, ndp31);
            // 3.1.1 White
            var ndp311 = CreateBox("NDP 3.1.1", "SURFACE", "سطح", ndp3Start, row3Y, cWhite, "200px");
            boxes.Add(ndp311); Connect(ndp31, ndp311);
            // 3.1.2 White
            var ndp312 = CreateBox("NDP 3.1.2", "AIR", "جو", ndp3Start, row4Y, cWhite, "200px");
            boxes.Add(ndp312); Connect(ndp311, ndp312);
            // Manual (Green dashed)
            var man1 = CreateBox("NDP Manual", null, null, ndp3Start, row5Y, cGreen, "200px");
            man1.BorderStyle = "dashed"; boxes.Add(man1); Connect(ndp312, man1);


            // 3.2 Orange
            ndp3Start += spacing;
            var ndp32 = CreateBox("NDP 3.2", "Sub-Surface", "تحت السطح", ndp3Start, row2Y, cOrange, "200px");
            boxes.Add(ndp32); Connect(ndp3, ndp32);
            // 3.2.1 Green
            var ndp321 = CreateBox("NDP 3.2.1", "Anti Doctrine", "المضادة", ndp3Start, row3Y, cGreen, "200px");
            boxes.Add(ndp321); Connect(ndp32, ndp321);
            // 3.2.1.1 Green Dashed
            var ndp3211 = CreateBox("NDP 3.2.1.1", "ASW Manual", null, ndp3Start, row4Y, cGreen, "200px");
            ndp3211.BorderStyle = "dashed"; boxes.Add(ndp3211); Connect(ndp321, ndp3211);
             // 3.2.2 Green
            var ndp322 = CreateBox("NDP 3.2.2", "Mine Warfare", "حرب الألغام", ndp3Start, row5Y, cGreen, "200px");
            boxes.Add(ndp322); Connect(ndp3211, ndp322);
            // 3.2.3 Green Dashed
            var ndp323 = CreateBox("NDP Layer", "Laying Procedures", null, ndp3Start, row6Y, cGreen, "200px");
            ndp323.BorderStyle = "dashed"; boxes.Add(ndp323); Connect(ndp322, ndp323);
            // Pink 3.2.3 below?
            var ndp323_pink = CreateBox("NDP 3.2.3", null, null, ndp3Start, row6Y + 150, cPink, "200px");
            boxes.Add(ndp323_pink); Connect(ndp323, ndp323_pink);


            // 3.3 White
            ndp3Start += spacing;
            var ndp33 = CreateBox("NDP 3.3", "Amphibious", "العمليات البرمائية", ndp3Start, row2Y, cWhite, "200px");
            boxes.Add(ndp33); Connect(ndp3, ndp33);


            // 3.4 Green
            ndp3Start += spacing;
            var ndp34 = CreateBox("NDP 3.4", "Naval Air Support", "الاسناد الجوي", ndp3Start, row2Y, cGreen, "200px");
            boxes.Add(ndp34); Connect(ndp3, ndp34);
            // 3.4.1 Yellow
            var ndp341 = CreateBox("NDP 3.4.1", "SUPPORT ORDER", "(NTP 06)", ndp3Start, row3Y, cYellow, "200px");
            boxes.Add(ndp341); Connect(ndp34, ndp341);
            // 3.4.2 Yellow
            var ndp342 = CreateBox("NDP 3.4.2", "OPERATIONS WITH", "(NTP 04)", ndp3Start, row4Y, cYellow, "200px");
            boxes.Add(ndp342); Connect(ndp341, ndp342);
             // 3.4.3 Yellow
            var ndp343 = CreateBox("NDP 3.4.3", "TASMO", null, ndp3Start, row5Y, cYellow, "200px");
            boxes.Add(ndp343); Connect(ndp342, ndp343);
             // 3.4.4 Yellow
            var ndp344 = CreateBox("NDP 3.4.4", "TACTICAL MANUAL", "(NTP 05)", ndp3Start, row6Y, cYellow, "200px");
            boxes.Add(ndp344); Connect(ndp343, ndp344);


            // 3.5 Orange
            ndp3Start += spacing;
            var ndp35 = CreateBox("NDP 3.5", "Operation Readiness", "الجاهزية العملياتية", ndp3Start, row2Y, cOrange, "200px");
            boxes.Add(ndp35); Connect(ndp3, ndp35);
             // 3.5.1 White
            var ndp351 = CreateBox("NDP 3.5.1", "OPERATION", "العمليات", ndp3Start, row3Y, cWhite, "200px");
            boxes.Add(ndp351); Connect(ndp35, ndp351);
             // 3.5.2 White
            var ndp352 = CreateBox("NDP 3.5.2", "Inspection", "التفتيش العملياتي", ndp3Start, row4Y, cWhite, "200px");
            boxes.Add(ndp352); Connect(ndp351, ndp352);


            // 3.6 Orange
            ndp3Start += spacing;
            var ndp36 = CreateBox("NDP 3.6", "Ship Org", "تنظيم السفن", ndp3Start, row2Y, cOrange, "200px");
            boxes.Add(ndp36); Connect(ndp3, ndp36);
            // 3.6.1 White
            var ndp361 = CreateBox("NDP 3.6.1", "DUTY ONBOARD", "الواجبات على متن السفينة", ndp3Start, row3Y, cWhite, "200px");
            boxes.Add(ndp361); Connect(ndp36, ndp361);
            // 3.6.2 White
            var ndp362 = CreateBox("NDP 3.6.2", "BILL MANUAL", "الاستعداد", ndp3Start, row4Y, cWhite, "200px");
            boxes.Add(ndp362); Connect(ndp361, ndp362);
            // 3.6.3 Green
            var ndp363 = CreateBox("NDP 3.6.3", "BATTLE EMPLOYMENT", "(NTP 02)", ndp3Start, row5Y, cGreen, "200px");
            boxes.Add(ndp363); Connect(ndp362, ndp363);
            // 3.6.4 White
            var ndp364 = CreateBox("NDP 3.6.4", "CO Guide Book", "(NQP 170)", ndp3Start, row6Y, cWhite, "200px");
            boxes.Add(ndp364); Connect(ndp363, ndp364);
             // 3.6.5 White
            var ndp365 = CreateBox("NDP 3.6.5", "(NQP 110)", null, ndp3Start, row6Y + 150, cWhite, "200px");
            boxes.Add(ndp365); Connect(ndp364, ndp365);


            // 3.7 Pink
            ndp3Start += spacing;
            var ndp37 = CreateBox("NDP 3.7", "Manual Safety", "السلامة", ndp3Start, row2Y, cPink, "200px");
            boxes.Add(ndp37); Connect(ndp3, ndp37);

            // Special orphan pink box 3.X at bottom left of 3.1
            var ndp3x = CreateBox("NDP 3X", "Special", null, 850, row6Y + 150, cPink, "200px");
            boxes.Add(ndp3x);


            // ================= NDP 4 Children =================
            var ndp41 = CreateBox("NDP 4.1", "Maintenance", "الصيانة", 2200, row2Y, cWhite); 
            boxes.Add(ndp41); Connect(ndp4, ndp41);
            var ndp4_med = CreateBox("NDP Manual", "MEDICAL", "الطبي", 2200, row3Y, cPink);
            boxes.Add(ndp4_med); Connect(ndp41, ndp4_med);


            // ================= NDP 6 Children =================
            var ndp61 = CreateBox("NDP 6.1", "COMMUNICATIONS", "الاتصالات", 2800, row2Y, cGreen);
            boxes.Add(ndp61); Connect(ndp6, ndp61);
            
            var ndp611 = CreateBox("NDP 6.1.1", "Procedures", "الإجراءات", 2800, row3Y, cGreen);
            boxes.Add(ndp611); Connect(ndp61, ndp611);

            var ndp62 = CreateBox("NDP 6.2", "Publications", "المنشورات", 2800, row4Y, cWhite);
            boxes.Add(ndp62); Connect(ndp611, ndp62);


             // ================= NDP 7 Children (Vertical List) =================
             double row7Start = row2Y;
             double vertSpace = 250; // More space for vertical list
             var ndp7list = new List<(string, string, string)>{
                 ("Semi-Annual Programs", "برامج التدريب", cGreen),
                 ("NDC Exercises", "قائمة التمارين", cGreen),
                 ("NDP 7.2.1", "NAVIGATIONAL", cGreen),
                 ("NADP 02", "MARITIME", cGreen),
                 ("NDP (NXP 204)", "SAFETY", cGreen),
                 ("NDP 1", "SYNTHETIC TRAINING", cGreen),
                 ("AKP 5", "COMMUNICATION", cGreen),
                 ("NDP 7.3", "DIVING", cGreen)
             };

             FlowBox parent7 = ndp7;
             foreach(var node in ndp7list) {
                 var bx = CreateBox(node.Item1, null, node.Item2, 3100, row7Start, node.Item3, "250px");
                 boxes.Add(bx);
                 Connect(parent7, bx);
                 parent7 = bx; // Daisy chain
                 row7Start += vertSpace;
             }

             // Loose Pink Boxes
             var taskList = CreateBox("Task List (NTL)", "List", "قائمة واجبات", 2800, row6Y, cPink);
             boxes.Add(taskList);

             var postManual = CreateBox("NDP 7.x", "Post/Center Manual", "قائمة المهام", 2800, row6Y + 150, cPink);
             boxes.Add(postManual);
             Connect(taskList, postManual);


            context.FlowBoxes.AddRange(boxes);
            context.SaveChanges();

            context.FlowConnections.AddRange(connections);
            context.SaveChanges();
            
            // Legend
             var legends = new List<LegendItem>
            {
                new LegendItem { Title = "عقائد تحمل درجة سري", BackgroundColor = "transparent", BorderColor = "red", BorderStyle = "dashed", OrderIndex = 1 },
                new LegendItem { Title = "عقائد/ مراجع معتمدة", BackgroundColor = "#FFFFFF", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 2 },
                new LegendItem { Title = "عقائد/ مراجع معتمدة بحاجة إلى مراجعة وتطوير", BackgroundColor = "#C1E1C1", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 3 },
                new LegendItem { Title = "عقائد/ مراجع قيد الإعداد", BackgroundColor = "#FADADD", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 4 },
                new LegendItem { Title = "عقائد/ موضوعات سيتم تضمينها في عقائد أخرى أثناء إعدادها", BackgroundColor = "#0070C0", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 5 },
                new LegendItem { Title = "مخاطبة جهات الاختصاص بشأن مدى الحاجة", BackgroundColor = "#FFFF00", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 6 },
                new LegendItem { Title = "مجال Domain", BackgroundColor = "#F4B084", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 7 },
                new LegendItem { Title = "عقيدة مترجمة باللغة العربية / الانجليزية", BackgroundColor = "#FFFFFF", BorderColor = "#000000", BorderStyle = "solid", OrderIndex = 8 }
            };
            context.LegendItems.AddRange(legends);
            context.SaveChanges();
            
            // Seed Metadata
            context.AppMetadata.Add(new AppMetadata { Key = "LastUpdated", Value = DateTime.Now.ToString("g") });
            context.SaveChanges();
    }
}
