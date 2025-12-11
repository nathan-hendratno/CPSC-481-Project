# Labyrinthian

A simple sliding-movement maze game where players navigate to the goal while leaving a visual path.
When finished, an AI using A* Search automatically displays the optimal path and compares it to the player's path.

Repo Link: https://github.com/nathan-hendratno/CPSC-481-Project

## 🚀 How to Run the Game

1. Download the zip file in the release link in the repo
2. Extract the zip file
3. Launch the CPSC 481 Project app file

Gameplay:

- Use WASD / Arrow Keys to move
- Player slides in a direction until hitting a wall
- Reach the end of the maze to finish
- AI will replay the optimal route using A*
- A score screen compares player path vs optimal path
- Result data would be logged into a CVS File

Dataset location: C:/Users/.../AppData/LocalLow/CompanyName/GameName/run_data.csv

## If you want to open in Unity editor

Unity 2022.x or later
(Other versions may work, but this is the tested configuration.)

### 🛠 Steps to run:

1. Clone or download this repository
2. Open Unity Hub → Add Project from Disk
3. Select the project folder
4. Open the project
5. Load GameScene.unity (default scene containing UI + Level loader)
6. Press Play ▶ in the Unity editor

## 📁 Project Structure & File Purpose

- PlayerController.cs       → Handles movement input and win condition logic
- PathCost.cs               → Tracks tiles visited + scoring (supports backtracking)
- PathTrail.cs              → Draws player's path in real time using LineRenderer
- AStarPathfinder.cs        → Handles A* pathfinding logic on the maze grid
- AStarDrawer.cs            → Draws optimal path replay + displays results
- GridScanner.cs            → Detects maze walls and walkable cells for A*
- GameplayUI.cs             → Controls in-game UI (pause, win screen, buttons)
- DataRunLogger.cs          → Logs the User's run into a CSV file

## 🧾 Dependencies (only required if opened in unity editor/hub)

Component
- Unity 2022+
- TextMeshPro (UI text requires TMP)
- LineRenderer Component (for player's path + A* replay)
- 2D Tilemap/Colliders
