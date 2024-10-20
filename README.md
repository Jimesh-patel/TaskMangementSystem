# Task Management System

## Introduction

The **Task Management System** is a web-based application built using **ASP.NET Core MVC** that helps teams manage their projects and tasks efficiently. It provides functionality for project leaders to create projects, assign tasks to team members, and track the progress of both tasks and projects. This system is designed to automate key processes like task and project status updates, allowing team members to focus on their work rather than manual tracking.

The application supports authentication to ensure that only authorized users can access the system. Team members can see their tasks, view upcoming work, and mark tasks as complete when done. Project and task statuses are managed automatically based on the project timelines and task completions.

---

## Features

### 1. Authentication
- The system includes user authentication, allowing users to securely log in and access their respective projects and tasks.
- Only registered users can create and manage projects or tasks.

### 2. Create Project
- Team leaders can create new projects by specifying details such as project name, description, start date, and end date.
- When a project is created, the project leader is assigned automatically.

### 3. Add Tasks and Assign to Team Members
- Team leaders can create tasks for a project and assign them to specific team members by email.
- Tasks include details like task title, description, and deadlines.

### 4. Show All Tasks or Today's Work
- Users can view a list of all tasks assigned to them or filter the view to show only tasks due today.
- This helps team members stay focused on their daily tasks.

### 5. Mark Task as Complete
- Once a task is completed, the assigned team member can mark it as "Complete."
- This updates the task's status in the system.

### 6. Manage Status of Task and Project Automatically
- The system automatically updates the status of tasks and projects based on deadlines and task completion.
  
  - **Task Status**: Tasks are marked as "Complete" when done, or remain "In Progress" otherwise.
  - **Project Status**:
    - **Not Started**: If the start date is in the future.
    - **In Process**: If the project has started but is not yet complete.
    - **Missed**: If the end date has passed without completing the project.

---

## User Roles

### 1. Team Leader
- Can create projects and tasks.
- Assigns tasks to team members.
- Views the status of all tasks and the overall project.
- Manages the project, including automatic status updates based on deadlines.

### 2. Team Member
- Views tasks assigned to them.
- Marks tasks as complete once finished.
- Sees their daily tasks or all pending tasks in a task list.
- Contributes to project progress by completing assigned tasks.

---

## Technology Stack

- **Backend**: ASP.NET Core MVC
- **Frontend**: HTML, CSS, Bootstrap
- **Database**: Entity Framework Core with SQL Server
