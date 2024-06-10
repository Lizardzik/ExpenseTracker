
# Expense Tracker

Expense Tracker is a web application that allows users to track their personal expenses. Each user can log in to their account and view, add, edit, and delete their own transactions. The application provides various features to help manage personal finances, such as categorizing expenses and displaying them in charts.

## Features

- **Multi-user Support**: Each user can log in and only view their own transactions.
- **Image Handling**: Ability to add images (stored in the database as byte[]) to transactions and display them.
- **Front-end Technologies**: The website is built using Bootstrap, CSS, and Syncfusion components.
- **Expense CRUD**: Users can create, read, update, and delete both expense categories and individual expenses.
- **Charts and Navigation**: Expenses are displayed using charts for better visualization, and a navbar helps with easy navigation.
- **User Sessions**: User identification is managed through sessions, allowing for secure and personalized experiences.

## Technologies Used

- **EntityFramework**: Used for database operations.
- **Syncfusion**: Used for front-end components, licensed for use in this project.
- **Bootstrap**: Used for responsive design and styling.
- **JavaScript**: Used for dynamic front-end functionality.
- **Sessions**: Used for user identification and management.
## Installation

1. **Download the project:**
    ```bash
    git clone https://github.com/Lizardzik/ExpenseTracker.git
    ```
2. **Open it in Microsoft Visual Studio:**
    ```bash
    in a repos folder
    ```
3. **Change the ConnectionString:**
    ```bash
   in appsettings to match you SQL server
    ```
4. **Use Enitity FrameWork to create database:**
    - Run database migrations or use provided scripts to set up the schema.
5. **Try application:**

## Usage

1. Open your web browser.
2. Register a new account.
3. Start tracking your expenses by adding new transactions, categorizing them, and viewing the summary in the provided charts.

## Development Time

I spent approximately 36 hours developing this project. It is far from perfect but im trying to improve.
