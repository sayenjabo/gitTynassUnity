using System.Collections.Generic;

/// <summary>
/// TynassIt — Global in-memory session state shared across all panels.
///
/// IMPORTANT: Your TynassApiClient already has its own SessionData class
/// (the MongoDB session model). This class is for UI/navigation state only
/// — the logged-in company, selected employee, active training, quiz progress.
///
/// TynassApiClient handles the token internally. Don't duplicate it here.
/// </summary>
public static class AppSession
{
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
    public static string EmployeeAvatarColor { get; set; } // hex string e.g. "#0048FF"
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