using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Đường dẫn chính xác tới file XML theo yêu cầu
        string filePath = @"D:\LTTQ\Quanlysinhvien\students.xml";

        // Kiểm tra file tồn tại trước khi đọc
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Lỗi: Không tìm thấy file tại đường dẫn '{filePath}'. Vui lòng kiểm tra lại!");
            return;
        }

        XDocument doc;
        try
        {
            doc = XDocument.Load(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi đọc file XML: {ex.Message}");
            return;
        }

        var students = doc.Element("Students")?.Elements("Student");
        if (students == null)
        {
            Console.WriteLine("File XML không đúng cấu trúc (thiếu thẻ gốc Students).");
            return;
        }

        int choice;
        do
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("             QUẢN LÝ SINH VIÊN (LINQ TO XML)      ");
            Console.WriteLine("==================================================");
            Console.WriteLine(" 1. Hiển thị danh sách tất cả sinh viên");
            Console.WriteLine(" 2. Danh sách sinh viên có GPA >= 8.0");
            Console.WriteLine(" 3. Danh sách sinh viên lớp CTK44");
            Console.WriteLine(" 4. Danh sách sinh viên nữ");
            Console.WriteLine(" 5. Sinh viên có GPA cao nhất");
            Console.WriteLine(" 6. Sinh viên có GPA thấp nhất");
            Console.WriteLine(" 7. Đếm tổng số lượng sinh viên");
            Console.WriteLine(" 8. Tính GPA trung bình của tất cả sinh viên");
            Console.WriteLine(" 9. Thống kê số lượng sinh viên theo lớp");
            Console.WriteLine("10. Sắp xếp danh sách sinh viên theo GPA giảm dần");
            Console.WriteLine("11. Tìm sinh viên có độ tuổi từ 20 đến 21");
            Console.WriteLine("12. Sinh viên CNTT và GPA >= 8.0");
            Console.WriteLine("13. Tìm kiếm sinh viên theo Mã sinh viên");
            Console.WriteLine("14. Lọc sinh viên theo tên lớp (sắp xếp GPA giảm dần)");
            Console.WriteLine("15. Top 3 sinh viên có GPA cao nhất");
            Console.WriteLine(" 0. Thoát chương trình");
            Console.WriteLine("==================================================");
            Console.Write("Nhập lựa chọn của bạn (0-15): ");

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Lựa chọn không hợp lệ! Vui lòng nhập số từ 0 đến 15.");
                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
                continue;
            }

            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    Console.WriteLine("--- CÂU 1: TOÀN BỘ DANH SÁCH SINH VIÊN ---");
                    foreach (var s in students) PrintStudent(s);
                    break;
                case 2:
                    Console.WriteLine("--- CÂU 2: SINH VIÊN CÓ GPA >= 8.0 ---");
                    foreach (var s in students.Where(s => double.Parse(s.Element("GPA")?.Value ?? "0") >= 8.0)) PrintStudent(s);
                    break;
                case 3:
                    Console.WriteLine("--- CÂU 3: SINH VIÊN LỚP CTK44 ---");
                    foreach (var s in students.Where(s => s.Element("Class")?.Value == "CTK44")) PrintStudent(s);
                    break;
                case 4:
                    Console.WriteLine("--- CÂU 4: DANH SÁCH SINH VIÊN NỮ ---");
                    foreach (var s in students.Where(s => s.Element("Gender")?.Value.Equals("Nu", StringComparison.OrdinalIgnoreCase) == true)) PrintStudent(s);
                    break;
                case 5:
                    Console.WriteLine("--- CÂU 5: SINH VIÊN CÓ GPA CAO NHẤT ---");
                    double maxGpa = students.Max(s => double.Parse(s.Element("GPA")?.Value ?? "0"));
                    foreach (var s in students.Where(s => double.Parse(s.Element("GPA")?.Value ?? "0") == maxGpa)) PrintStudent(s);
                    break;
                case 6:
                    Console.WriteLine("--- CÂU 6: SINH VIÊN CÓ GPA THẤP NHẤT ---");
                    double minGpa = students.Min(s => double.Parse(s.Element("GPA")?.Value ?? "0"));
                    foreach (var s in students.Where(s => double.Parse(s.Element("GPA")?.Value ?? "0") == minGpa)) PrintStudent(s);
                    break;
                case 7:
                    Console.WriteLine("--- CÂU 7: TỔNG SỐ LƯỢNG SINH VIÊN ---");
                    Console.WriteLine($"Tổng số sinh viên: {students.Count()}");
                    break;
                case 8:
                    Console.WriteLine("--- CÂU 8: GPA TRUNG BÌNH ---");
                    Console.WriteLine($"GPA trung bình chung: {students.Average(s => double.Parse(s.Element("GPA")?.Value ?? "0")):F2}");
                    break;
                case 9:
                    Console.WriteLine("--- CÂU 9: THỐNG KÊ SỐ LƯỢNG THEO LỚP ---");
                    foreach (var group in students.GroupBy(s => s.Element("Class")?.Value))
                    {
                        Console.WriteLine($"{group.Key}: {group.Count()} sinh viên");
                    }
                    break;
                case 10:
                    Console.WriteLine("--- CÂU 10: SẮP XẾP THEO GPA GIẢM DẦN ---");
                    foreach (var s in students.OrderByDescending(s => double.Parse(s.Element("GPA")?.Value ?? "0"))) PrintStudent(s);
                    break;
                case 11:
                    Console.WriteLine("--- CÂU 11: SINH VIÊN TỪ 20 ĐẾN 21 TUỔI ---");
                    foreach (var s in students.Where(s => int.TryParse(s.Element("Age")?.Value, out int age) && age >= 20 && age <= 21)) PrintStudent(s);
                    break;
                case 12:
                    Console.WriteLine("--- CÂU 12: NGÀNH CNTT VÀ GPA >= 8.0 ---");
                    foreach (var s in students.Where(s => s.Element("Major")?.Value.Equals("Cong nghe thong tin", StringComparison.OrdinalIgnoreCase) == true && double.Parse(s.Element("GPA")?.Value ?? "0") >= 8.0)) PrintStudent(s);
                    break;
                case 13:
                    Console.WriteLine("--- CÂU 13: TÌM KIẾM THEO MÃ SINH VIÊN ---");
                    Console.Write("Nhập mã sinh viên cần tìm (Ví dụ: SV001): ");
                    string searchId = Console.ReadLine()?.Trim();
                    var sv = students.FirstOrDefault(s => s.Attribute("id")?.Value.Equals(searchId, StringComparison.OrdinalIgnoreCase) == true);
                    if (sv != null) PrintStudent(sv);
                    else Console.WriteLine("Không tìm thấy sinh viên.");
                    break;
                case 14:
                    Console.WriteLine("--- CÂU 14: LỌC THEO TÊN LỚP VÀ SẮP XẾP GPA GIẢM DẦN ---");
                    Console.Write("Nhập tên lớp cần lọc (Ví dụ: CTK44): ");
                    string searchClass = Console.ReadLine()?.Trim();
                    var q14 = students.Where(s => s.Element("Class")?.Value.Equals(searchClass, StringComparison.OrdinalIgnoreCase) == true)
                                      .OrderByDescending(s => double.Parse(s.Element("GPA")?.Value ?? "0"));
                    if (q14.Any()) foreach (var s in q14) PrintStudent(s);
                    else Console.WriteLine($"Không tìm thấy sinh viên thuộc lớp '{searchClass}'.");
                    break;
                case 15:
                    Console.WriteLine("--- CÂU 15: TOP 3 SINH VIÊN GPA CAO NHẤT ---");
                    foreach (var s in students.OrderByDescending(s => double.Parse(s.Element("GPA")?.Value ?? "0")).Take(3)) PrintStudent(s);
                    break;
                case 0:
                    Console.WriteLine("Đang thoát chương trình...");
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }

            if (choice != 0)
            {
                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
            }
        } while (choice != 0);
    }

    static void PrintStudent(XElement s)
    {
        string id = s.Attribute("id")?.Value;
        string name = s.Element("Name")?.Value;
        string gender = s.Element("Gender")?.Value;
        string age = s.Element("Age")?.Value;
        string lop = s.Element("Class")?.Value;
        string major = s.Element("Major")?.Value;
        string gpa = s.Element("GPA")?.Value;

        Console.WriteLine($"[ID: {id}] | Tên: {name,-16} | Giới tính: {gender,-3} | Tuổi: {age} | Lớp: {lop,-5} | Ngành: {major,-22} | GPA: {gpa}");
    }
}