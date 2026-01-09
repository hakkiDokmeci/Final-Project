# 🎓 Exam Security System

### Backend & Frontend Demonstration Project

## 📌 Project Overview

The **Exam Security System** is a demonstration project designed to showcase how backend-driven business rules can be used to enforce exam integrity. The system focuses on student check-in validation, seat compliance, identity verification, and violation logging, with a clear separation between backend logic and frontend presentation.

This project was developed as part of a **Software Validation & Testing / Backend-Oriented Course Project**, with emphasis on:
* Backend rule enforcement
* Clean architecture
* Unit testing
* Minimal UI dependency

---

## 🧠 Core Concept

> **"All critical rules are enforced in the backend."**

The frontend only displays data and triggers actions; it **cannot** override or manipulate system decisions.

---

## 🏗️ System Architecture

### 🔙 Backend
* **Framework:** ASP.NET Core Web API
* **ORM:** Entity Framework Core (Database-First)
* **Testing:** MSTest for unit testing
* **Architecture:** Service-based business logic
* **ML Integration:** ML abstraction layer (stubbed)

### 🖥️ Frontend
* Simple UI with dummy/static data
* Used only to demonstrate flow and screens
* **No critical logic implemented on the frontend**

---

## ⚙️ Backend Features

### 🔐 Authentication & Authorization
* Role-based access control (**Admin**, **Proctor**).
* Backend-enforced permissions using authorization attributes.
* Zero frontend trust for sensitive operations.

### 📝 Exam / Room / Seating Management
* Exams are defined with:
    * Room
    * Date & Time
    * Seating dimensions (Rows & Columns)
* **Exam Roster:** Controls which students are eligible.
* **Seating Plans:** Stored in the backend using seat codes (e.g., `A1`, `B2`).

### ✅ Check-in Workflow
The backend processes each check-in request using the following inputs:
1.  Exam ID
2.  Student ID
3.  Observed Seat Code
4.  Captured Image Reference
5.  Optional Notes

**Backend Logic Decisions:**
* A student cannot check in twice for the same exam.
* **Seat Compliance:** Checks assigned seat vs. observed seat.
* **Identity Verification:** Performed via ML abstraction.
* **Automatic Status Assignment:** `CHECKED_IN` or `VIOLATION`.
* All check-ins are timestamped using server-side UTC time.

### 🤖 Identity Verification (Computer Vision – Abstracted)
Identity verification is handled via an interface: `IFaceMatchService`. The system simulates face verification using similarity score logic.

* **Possible Results:** `MATCH`, `NO_MATCH`, `NOT_RUN`.
* **Implementation:** A stub implementation (`FakeFaceMatchService`) is used to:
    * Demonstrate ML integration architecture.
    * Enable deterministic unit testing.
    * Avoid dependency on real ML infrastructure during development.

### 🚨 Violation Logging
Violations are linked directly to the related check-in record and are automatically created when:
* Identity verification fails.
* Student sits in an incorrect seat.

**Violation Record Details:**
* **Type:** `IDENTITY_MISMATCH`, `WRONG_SEAT`, `OTHER`
* Detailed explanation
* Optional evidence reference
* Timestamp

---

## 🧪 Testing Strategy

### Unit Tests
* Written using **MSTest**.
* Executed with **EF Core InMemory** database.
* Focused entirely on critical business rules.

### Covered Scenarios
- [x] Valid check-in (No violation)
- [x] Duplicate check-in prevention
- [x] Wrong seat violation
- [x] Identity mismatch violation

> **Testing Philosophy:** If a rule is important for exam security, it must be testable and enforced in the backend.

---

## 🗄️ Database Design

* **Approach:** Database-First (SQL Server schema created manually).
* **Constraints Enforced:**
    * Unique check-ins per Exam/Student.
    * Valid enum-like values for status/results.
    * Referential integrity.
* **Reporting:** Views included for exam summary statistics and detailed check-in/violation reports.

---

## 🎯 Project Goals Achieved

✅ **Backend-driven business rules** ✅ **Clear separation of concerns** ✅ **Testable and extensible architecture** ✅ **ML-ready design via abstraction** ✅ **Minimal but sufficient frontend**
