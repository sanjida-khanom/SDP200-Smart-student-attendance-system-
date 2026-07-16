# Smart Student Attendance System

A **Windows desktop application** that automates student attendance using **QR code scanning through a live webcam feed**. Instead of calling names or passing a sheet, each student is issued a QR code at registration; scanning it in front of the camera pulls up their record and stamps their attendance with an entry time and a captured photo.

Built as an SDP200 software development project using **C# WinForms** and **MySQL**.

## Overview

The system covers the full attendance lifecycle:

1. An admin logs in.
2. Students are registered with their details and photo — a **QR code is generated** from their record.
3. At attendance time, a student holds their QR code to the webcam. The app decodes it in real time, auto-fills their details, captures their photo, and records the entry time.
4. Attendance and registration records are viewable in tables and **printable as reports**.

## Features

### Admin login
`LoginForm` authenticates admins against the `login_tb` table before granting access to the system.

### Student registration
`RegistrationForm` captures a student's ID, name, father's name, email, date of birth, class, phone number, gender, and photo, storing the record (with the image as a BLOB) in `registration_tb`. A **QR code is generated** for the student using **QRCoder**.

### Live QR scanning
`ScanForm` is the core of the system:
- Enumerates all connected webcams via **AForge.Video.DirectShow** and lists them in a dropdown
- Streams the live feed frame-by-frame through the `NewFrame` event handler
- Decodes QR codes in real time using **ZXing.Net**
- Auto-populates the student's details from the decoded value
- Captures the student's photo and stamps the current date and time
- Writes the record to `attendance_tbl`

### Attendance & student records
- `attendanceForm` — displays all registered students from `registration_tb`
- `StudentInfo` — displays all attendance entries from `attendance_tbl`

### Report printing
`Receipt.cs` and `DGVPrinter.cs` provide **DataGridView printing**, so attendance sheets can be printed or exported directly from the grid. Microsoft ReportViewer is also referenced for report rendering.

### Navigation
`MainForm` is the hub, launching the Registration, Scan, Attendance, and Student Info forms.

## Tech stack

- **Language:** C#
- **Framework:** .NET Framework 4.5.2, Windows Forms
- **Database:** MySQL (via MySql.Data 8.0.28 connector)
- **Camera capture:** AForge.NET 2.2.5 (`AForge`, `AForge.Video`, `AForge.Video.DirectShow`)
- **QR generation:** QRCoder 1.4.3
- **QR decoding:** ZXing.Net 0.16.8
- **Reporting:** Microsoft.ReportViewer.WinForms 12.0, custom `DGVPrinter`

## Project structure

```
SmartAttendance/
│
├── Program.cs                    # Application entry point
├── LoginForm.cs                  # Admin authentication
├── MainForm.cs                   # Navigation hub
├── RegistrationForm.cs           # Student registration + QR generation
├── ScanForm.cs                   # Live webcam QR scanning + attendance capture
├── attendanceForm.cs             # Registered students view
├── StudentInfo.cs                # Attendance records view
├── Receipt.cs                    # Report/receipt rendering
├── DGVPrinter.cs                 # DataGridView printing helper
├── *.Designer.cs                 # Auto-generated form layouts
├── *.resx                        # Form resources
├── App.config                    # .NET runtime config
├── packages.config               # NuGet package manifest
└── WindowsFormsApp17.csproj      # Project file
```

## Database schema

The app connects to a MySQL database named **`user_info`** with three tables.

```sql
CREATE DATABASE user_info;
USE user_info;

-- Admin accounts
CREATE TABLE login_tb (
    UserName VARCHAR(50) PRIMARY KEY,
    Password VARCHAR(50) NOT NULL
);

-- Registered students
CREATE TABLE registration_tb (
    ID           VARCHAR(50) PRIMARY KEY,
    Name         VARCHAR(100),
    FatherName   VARCHAR(100),
    EmailAddress VARCHAR(100),
    DateOfBirth  VARCHAR(50),
    Class        VARCHAR(50),
    PhoneNumber  VARCHAR(20),
    Gender       VARCHAR(10),
    Photo        LONGBLOB
);

-- Attendance entries
CREATE TABLE attendance_tbl (
    RecordID     INT AUTO_INCREMENT PRIMARY KEY,
    ID           VARCHAR(50),
    Name         VARCHAR(100),
    FatherName   VARCHAR(100),
    EmailAddress VARCHAR(100),
    DateOfBirth  VARCHAR(50),
    Class        VARCHAR(50),
    PhoneNumber  VARCHAR(20),
    Gender       VARCHAR(10),
    InTime       VARCHAR(50),
    Photo        LONGBLOB
);

-- Seed an admin account so you can log in
INSERT INTO login_tb (UserName, Password) VALUES ('admin', 'admin123');
```

> Photos are stored as `LONGBLOB` columns, inserted via a parameterised `@photo` value.

## Prerequisites

- **Windows 10/11**
- **Visual Studio 2019 or 2022** with the **.NET desktop development** workload
- **.NET Framework 4.5.2** developer pack
- **MySQL Server** — XAMPP, WAMP, or a standalone install
- **A webcam** (built-in or USB) for the scanning feature
- **Microsoft ReportViewer 2012 Runtime** — required for the reporting references

## Configuration

The connection string is hardcoded in each form's constructor:

```csharp
con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
```

If your MySQL setup uses different credentials, update this line in **all five** files: `LoginForm.cs`, `RegistrationForm.cs`, `ScanForm.cs`, `attendanceForm.cs`, and `StudentInfo.cs`.

## How to run

### Step 1 — Set up the database
Start MySQL (via the XAMPP control panel or your service manager), then run the schema SQL above in phpMyAdmin or MySQL Workbench.

### Step 2 — Open the project
Open `WindowsFormsApp17.csproj` in Visual Studio.

### Step 3 — Restore NuGet packages
Right-click the solution → **Restore NuGet Packages**. This pulls in AForge, QRCoder, ZXing.Net, and MySql.Data as listed in `packages.config`.

If restore doesn't resolve everything, install them manually from the **Package Manager Console**:

```powershell
Install-Package AForge -Version 2.2.5
Install-Package AForge.Video -Version 2.2.5
Install-Package AForge.Video.DirectShow -Version 2.2.5
Install-Package QRCoder -Version 1.4.3
Install-Package ZXing.Net -Version 0.16.8
Install-Package MySql.Data -Version 8.0.28
```

### Step 4 — Build and run
Press **F5** (or **Build → Build Solution**, then run).

### Step 5 — Use the app
1. Log in with the seeded admin account (`admin` / `admin123`).
2. **Registration** — add a student and generate their QR code.
3. **Scan** — pick your webcam from the dropdown, click start, and hold a QR code up to the camera. Attendance is recorded automatically on a successful decode.
4. **Attendance / Student Info** — view and print the records.

## Troubleshooting

| Problem | Fix |
|---------|-----|
| Camera dropdown is empty | No webcam detected. Check Device Manager, and make sure no other app (Zoom, Teams, Camera) is holding the device. |
| `Unable to connect to any of the specified MySQL hosts` | MySQL isn't running, or the connection string doesn't match your setup. |
| `Could not load file or assembly 'MySql.Data'` | NuGet packages didn't restore. Run the restore step above. |
| ReportViewer reference errors | Install the Microsoft ReportViewer 2012 Runtime. |
| QR codes don't decode | Improve lighting, hold the code steady and flat, and keep it fully inside the frame. |
| App won't start on another PC | .NET Framework 4.5.2 runtime is missing. |

## Known issues & improvements

Being upfront about the gaps here — these are the natural next steps:

1. **SQL injection vulnerability.** Queries are built with string concatenation:
   ```csharp
   cmd.CommandText = "SELECT * FROM login_tb WHERE UserName='" + textBox1.Text + "' AND Password='" + textBox2.Text + "'";
   ```
   Entering `' OR '1'='1` as the username bypasses the login entirely. The fix is parameterised queries throughout:
   ```csharp
   cmd.CommandText = "SELECT * FROM login_tb WHERE UserName=@user AND Password=@pass";
   cmd.Parameters.AddWithValue("@user", textBox1.Text);
   cmd.Parameters.AddWithValue("@pass", textBox2.Text);
   ```
   The photo insert already uses `@photo` correctly — the same pattern just needs applying to every other field.

2. **Passwords stored in plaintext** in `login_tb`. Hashing with BCrypt or PBKDF2 would be standard.

3. **Connection string duplicated across five files** — and it contains the DB credentials. Move it into `App.config`'s `<connectionStrings>` section and read it from one place.

4. **String concatenation adds stray spaces.** In the insert statements, values like `"'" + ID_text.Text + " '"` include leading/trailing spaces inside the quotes, so `"101"` gets stored as `" 101 "`. This will cause lookups to miss. Parameterising fixes this as a side effect.

5. **No duplicate-attendance guard** — a student can scan repeatedly and create multiple entries for the same day. A check on `(ID, date)` before insert would prevent it.

6. **Camera resource cleanup** — `FinalFrame` should be stopped in `FormClosing` to release the webcam when the form closes.

7. **Legacy target framework.** .NET Framework 4.5.2 is out of support; migrating to .NET 8 with Windows Forms would modernize the stack.

## Concepts demonstrated

- **Real-time video processing** — frame-by-frame webcam capture and decoding
- **Computer vision** — QR code detection and decoding from a live feed
- **CRUD database operations** over MySQL with BLOB image storage
- **Event-driven desktop UI** — WinForms event handling and multi-form navigation
- **Report generation** — printable data grids
