# Calendar Booking Application

This application is a simple console command-line application for managing appointment bookings. it's using
.net 8.0,
SqlExpress and
Entity Framework

## Commands

- `ADD DD/MM hh:mm` - Add an appointment.
- `DELETE DD/MM hh:mm` - Remove an appointment.
- `FIND DD/MM` - Find a free timeslot for the day.
- `KEEP hh:mm` - Keep a timeslot for any day.

## Requirements

- The time slot is always 30 minutes.
- The acceptable time is between 9 AM and 5 PM, except from 4 PM to 5 PM on each second day of the third week of any month.

## Assumptions

- time format said to be hh:mm but it needs timeSpecifier (tt : AM/PM), so it is assumed that time will be excepted in either HH:mm or hh:mm tt format. Example ADD 15/07 15:00 or ADD 15/07 03:00 PM
- Since acceptance time is between 9 AM and 5PM, it is assumed that there would be available slot from 5 PM to 5.30 PM.

## Database

The application uses SQL Server Express LocalDB for state storage.

## Improvements

Given more time (2 days), the following improvements could be made:
1. **Error Handling:** Enhance error handling and validation.
2. **Logging:** Add logging for better diagnostics.
3. **User Interface:** Improve user feedback on command execution, may be basic React GUI and enhanced commands such as DELETE ALL KEEP.
4. **Web API:** Create web api for commands which connects to database.
5. **Concurrency:** Handle concurrent bookings gracefully.
6. **Database Migrations:** Implement database migrations for schema changes.
7. **Performance:** Optimize database queries and improve performance.
8. **Configuration:** Externalize configuration settings (e.g., database connection string).
9. **Comprehensive Unit Tests:** Add more unit tests for different scenarios.
10. **Documentation:** Improve and expand documentation for users and developers.

## Setup

1. Clone the repository.
2. Build the project using .NET CLI or Visual Studio 2022.
3. Might need to run/ create migration scipts
4. Run the application and provide Commands.

