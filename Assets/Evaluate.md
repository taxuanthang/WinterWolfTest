Project Design Review
Overview
    This document evaluates the current project structure and design, including advantages, disadvantages, and suggestions for improvement.

Advantages
1. Clear Structure
    The project separates major systems such as:
        - GameManager
        - BoardController
        - LevelCondition
        - UI
        - ScriptableObject data
    This makes the project easy to understand and follow.

2. Use of ScriptableObjects
    Using ScriptableObjects (GameSettings, ItemSkinDatabase, PrefabDatabase) is a good practice.
    It helps:
        - Adjust data easily
        - Avoid hard-coded values
        - Make the project more scalable

3. Event Usage
    Using events (e.g., StateChangedAction) helps reduce tight coupling between systems like UI and gameplay.

4. Object Pooling
    ItemPool helps reduce Instantiate/Destroy calls and improves performance, especially for mobile.

Disadvantages
1. GameManager Handles Too Many Responsibilities
    GameManager currently manages:
        - Game state
        - Level loading
        - Board setup
        - UI setup
        - Tween pause/resume
    This may cause difficulty in maintaining the code when the project grows.

2. Some Runtime Allocations
    Board logic uses:
        - LINQ
        - Dictionary
        - List → ToArray
    These may create unnecessary allocations and could affect performance on low-end devices.

3. Resources.Load Usage
    Using Resources.Load is not recommended for large-scale projects. It would be better to use serialized references or Addressables.


Suggestions
    - Convert code into structure Model-View-Presenter.
    - Reduce LINQ and unnecessary allocations in gameplay logic.
    - Split GameManager responsibilities into smaller classes.
    - Organize folders clearly by feature (Core, Board, UI, Data, etc.).
    - Project by now create Managers(GameObject) by code then add scipt(if needed) into that which is good to avoid conflict when many people code togethers. But i prefers to have seperate GameObject in the scene at begining and if GameManagers check it not existed then it will create by code. The reason is for developer to control how many Manager and View(GameObject) exist in the game. 
