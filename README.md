# RBoard - Chess Application

![GitHub License](https://img.shields.io/github/license/ruzen42/rboard?style=for-the-badge&color=red)
![GitHub Release](https://img.shields.io/github/v/release/ruzen42/rboard?style=for-the-badge&color=red)

![GitHub watchers](https://img.shields.io/github/watchers/ruzen42/rboard?style=for-the-badge&color=green)
![GitHub contributors](https://img.shields.io/github/contributors/ruzen42/rboard?style=for-the-badge&color=red)

RBoard is a cross-platform chess application developed using Avalonia UI and .NET 9.0. The application implements classic chess rules with board and pieces visualization.

## 🚀 Features

- ♟️ (Un)Complete chess rules implementation for all pieces
- 🖥️ Cross-platform interface (Windows, Linux, macOS)
- 🎨 Theme support (Material) 
- 🔄 Turn-based move system
- 📊 Move visualization
- 🔄 Game reset at any time

## 📦 Installation

### Requirements
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Avalonia UI (automatically installed via NuGet)

### Build from source
1. Clone the repository:
   ```sh
   git clone https://github.com/ruzen42/rboard.git
   cd rboard

    Restore dependencies:
    sh

dotnet restore

Run the application:
sh

    dotnet run

Release build
sh

dotnet publish -c Release -r win-x64 --self-contained true

(replace win-x64 with your platform: linux-x64, osx-x64 etc.)
🎮 How to Play

    Click on the piece you want to move

    Click on the target square

    If the move is valid, the piece will move

    Turn switches to the opponent

Hotkeys

    Reset game: "Start New Game" button

    Cancel selection: Click the same piece or empty square

🛠 Technical Details

    Language: C# 10

    Framework: .NET 9.0

    UI: Avalonia 11

    Dependencies:

        Avalonia.Desktop

        Avalonia.Themes.Fluent

        Material.Avalonia

        Gera.Chess (for chess piece symbols)

📂 Project Structure

rboard/
├── Program.cs          - Application entry point
├── App.axaml.cs        - Avalonia configuration
├── MainWindow.axaml.cs - Main window and UI logic
├── MainWindow.axaml    - Main window
├── Logic.cs            - Chess rules implementation
└── rboard.csproj       - Project file

🤝 Contributing

Pull requests and issues are welcome. Before making changes:

    Create an issue for discussion

    Fork the repository

    Create a feature branch

    Submit a pull request

📜 License

This project is licensed under the GNU General Public License v3.0. See LICENSE file for details.

Developed with ♥ by ruzen42
