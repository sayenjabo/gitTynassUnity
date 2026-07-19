using System.Collections.Generic;

public static class AppSession
{
    // ─── Device — belongs to headset, never cleared on logout ─────────────────
    public static string DeviceToken { get; set; }
    public static string CompanyId   { get; set; }
    public static string CompanyName { get; set; }

    // ─── Company ──────────────────────────────────────────────────────────────
    public static string CompanyEmail { get; set; }
    public static List<TrainingData> AssignedTrainings { get; set; }

    // ─── Active employee ──────────────────────────────────────────────────────
    public static string EmployeeId         { get; set; }
    public static string EmployeeName       { get; set; }
    public static string EmployeeRole       { get; set; }
    public static string EmployeeInitials   { get; set; }
    public static string EmployeeAvatarColor{ get; set; }
    public static bool   IsGuest            { get; set; }

    // ─── Active training ──────────────────────────────────────────────────────
    public static TrainingData ActiveTraining { get; set; }

    // ─── Quiz session ─────────────────────────────────────────────────────────
    public static int   QuizScore        { get; set; }
    public static int   QuizCorrect      { get; set; }
    public static int   QuizTotal        { get; set; }
    public static int   QuizCurrentIndex { get; set; }
    public static float QuizElapsedSecs  { get; set; }

    // ─── Clear on employee logout — keep device data ──────────────────────────
    public static void Clear()
    {
        // DeviceToken, CompanyId, CompanyName belong to the headset — do NOT clear
        CompanyEmail        = null;
        AssignedTrainings   = null;
        EmployeeId          = null;
        EmployeeName        = null;
        EmployeeRole        = null;
        EmployeeInitials    = null;
        EmployeeAvatarColor = null;
        IsGuest             = false;
        ActiveTraining      = null;
        QuizScore           = 0;
        QuizCorrect         = 0;
        QuizTotal           = 0;
        QuizCurrentIndex    = 0;
        QuizElapsedSecs     = 0f;
    }
}
