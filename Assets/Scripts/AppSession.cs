using System.Collections.Generic;

public static class AppSession
{
    // ─── Device ───────────────────────────────────────────────────────────────
    public static string DeviceToken { get; set; }

    // ─── Company (set after CompanyLogin) ─────────────────────────────────────
    public static string CompanyId { get; set; }
    public static string CompanyName { get; set; }
    public static string CompanyEmail { get; set; }
    public static List<TrainingData> AssignedTrainings { get; set; }

    // ─── Active employee (set in EmployeePickerPanel) ─────────────────────────
    public static string EmployeeId { get; set; }
    public static string EmployeeName { get; set; }
    public static string EmployeeRole { get; set; }
    public static string EmployeeInitials { get; set; }
    public static string EmployeeAvatarColor { get; set; }
    public static bool IsGuest { get; set; }

    // ─── Active training (set in ModulesPanel) ────────────────────────────────
    public static TrainingData ActiveTraining { get; set; }

    // ─── Active quiz session ──────────────────────────────────────────────────
    public static int QuizScore { get; set; }
    public static int QuizCorrect { get; set; }
    public static int QuizTotal { get; set; }
    public static int QuizCurrentIndex { get; set; }
    public static float QuizElapsedSecs { get; set; }

    // ─── Clear on logout ──────────────────────────────────────────────────────
    public static void Clear()
    {
        DeviceToken = null;
        CompanyId = null;
        CompanyName = null;
        CompanyEmail = null;
        AssignedTrainings = null;
        EmployeeId = null;
        EmployeeName = null;
        EmployeeRole = null;
        EmployeeInitials = null;
        EmployeeAvatarColor = null;
        IsGuest = false;
        ActiveTraining = null;
        QuizScore = 0;
        QuizCorrect = 0;
        QuizTotal = 0;
        QuizCurrentIndex = 0;
        QuizElapsedSecs = 0f;
    }
}
