🎓 Exam Security System

Backend & Frontend Demonstration Project

📌 Project Overview

The Exam Security System is a demonstration project designed to showcase how backend-driven business rules can be used to enforce exam integrity.
The system focuses on student check-in validation, seat compliance, identity verification, and violation logging, with a clear separation between backend logic and frontend presentation.

This project was developed as part of a Software Validation & Testing / Backend-Oriented Course Project, with emphasis on:

Backend rule enforcement

Clean architecture

Unit testing

Minimal UI dependency

🧠 Core Concept

All critical rules are enforced in the backend.
The frontend only displays data and triggers actions; it cannot override or manipulate system decisions.

🏗️ System Architecture
Backend

ASP.NET Core Web API

Entity Framework Core (Database-First)

MSTest for unit testing

Service-based business logic

ML abstraction layer (stubbed)

Frontend

Simple UI with dummy/static data

Used only to demonstrate flow and screens

No critical logic implemented on the frontend

⚙️ Backend Features
🔐 Authentication & Authorization

Role-based access control (Admin, Proctor)

Backend-enforced permissions using authorization attributes

No frontend trust for sensitive operations

📝 Exam / Room / Seating Management

Exams are defined with:

Room

Date & time

Seating dimensions (rows & columns)

Exam roster controls which students are eligible

Seating plans are stored in the backend using seat codes (e.g., A1, B2)

✅ Check-in Workflow

The backend processes each check-in request using the following inputs:

Exam ID

Student ID

Observed seat code

Captured image reference

Optional notes

Backend decisions include:

A student cannot check in twice for the same exam

Seat compliance check (assigned vs observed seat)

Identity verification via ML abstraction

Automatic status assignment:

CHECKED_IN

VIOLATION

Each check-in is timestamped using server-side UTC time.

🤖 Identity Verification (Computer Vision – Abstracted)

Identity verification is handled via an interface: IFaceMatchService

The system simulates face verification using similarity score logic

Results:

MATCH

NO_MATCH

NOT_RUN

A stub implementation (FakeFaceMatchService) is used to:

Demonstrate ML integration

Enable deterministic unit testing

Avoid dependency on real ML infrastructure

🚨 Violation Logging

Violations are automatically created when:

Identity verification fails

Student sits in an incorrect seat

Each violation includes:

Violation type (IDENTITY_MISMATCH, WRONG_SEAT, OTHER)

Detailed explanation

Optional evidence reference

Timestamp

Violations are linked directly to the related check-in record.

🧪 Testing Strategy
Unit Tests

Written using MSTest

Executed with EF Core InMemory database

Focus on critical business rules

Covered Scenarios:

Valid check-in (no violation)

Duplicate check-in prevention

Wrong seat violation

Identity mismatch violation

Testing Philosophy

If a rule is important for exam security, it must be testable and enforced in the backend.

🗄️ Database Design

SQL Server schema created manually

Database-first approach

Constraints enforce:

Unique check-ins per exam/student

Valid enum-like values for status/results

Referential integrity

Reporting views are included for:

Exam summary statistics

Detailed check-in and violation reports

🎯 Project Goals Achieved

✅ Backend-driven business rules

✅ Clear separation of concerns

✅ Testable and extensible architecture

✅ ML-ready design via abstraction

✅ Minimal but sufficient frontend
