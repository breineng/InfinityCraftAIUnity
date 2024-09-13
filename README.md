# Demo Game of Item Generation Using AI

## Description

This repository contains a Unity project of a demo game that showcases item generation technology using neural networks.

In the game, you can combine two items to generate a new one:

- **3D Model Generation**: Uses the **Genie** neural network by **Luma AI**.
- **Item Name Generation**: Utilizes **ChatGPT 4o-mini**.
- **Physical Properties**: Assigned using **PhysicsMaterial**.
- **Sound Effects**: Appropriate falling sounds are selected by the neural network.

At the start of the game, you will be prompted to log in to **Genie** and optionally enter your **OpenAI Key**.

## Download and Play the Build

You can download the ready-to-play build from the [Releases](https://github.com/Temka193/InfinityCraft/releases) section.

1. Go to the [Releases](https://github.com/Temka193/InfinityCraft/releases) page.
2. Download the latest build.
3. Unzip the downloaded file.
4. Run the **InfinityCraft.exe** to start the game.

## How to Play

1. Launch the game.
2. Log in to **Genie** when prompted.
3. *(Optional)* Enter your **OpenAI Key**.
4. Place two items on the two platforms in the combiner.
5. Press the **"Combine"** button.
6. Wait for the generated item to appear on the third platform.
7. Explore the new item and its properties.

## Installation from Source

If you wish to explore or modify the source code:

1. Clone the repository:

   ```bash
      git clone https://github.com/Temka193/InfinityCraft.git
   ```
3. Open the project in Unity (version 2022.3 or higher).
4. Open and run the *Simple Garage/Scenes/Garage Scene*.

## Screenshots

![image](https://github.com/user-attachments/assets/a8fabe89-58d9-470f-9aa6-3090406d7f6d)
![image](https://github.com/user-attachments/assets/ca0a6435-e36b-4aec-ac91-11e5c826fcfa)


## Used Assets

- [LumaAI Genie C# Library](https://github.com/Temka193/LumaAIGenieSharp)
- [Advanced-FirstPerson-Controller](https://github.com/moe4b-professional/Advanced-FirstPerson-Controller)
- [Runtime OBJ Importer](https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547)
- [PhysSound Physics Audio System](https://discussions.unity.com/t/open-source-physsound-physics-audio-system/585439)
- [UnityWebBrowser](https://github.com/Voltstro-Studios/UnityWebBrowser)
- [Simple Garage](https://assetstore.unity.com/packages/3d/props/interior/simple-garage-197251)

## Requirements
- Unity 2022.3 or higher (for running from source).
- Internet connection for neural network functionalities.

## Feedback
If you have any questions or suggestions, please create an issue in this repository.
