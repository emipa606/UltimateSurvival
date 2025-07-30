# Copilot Instructions: RimWorld Modding Project

## Mod Overview and Purpose

This RimWorld mod introduces barbed wire defenses to the game, enhancing security and tactical elements available to players. The addition of barbed wire brings new strategic possibilities in managing colony defense, especially against raiders and wild animals.

## Key Features and Systems

- **Barbed Wire Construction:** allows players to build barbed wire fences that harm enemies, adding a layer of difficulty for intruders.
- **Automatic Rearm Mechanism:** barbed wires can be rearmed automatically after they spring, which ensures continued protection without manual intervention.
- **Trap Awareness:** pawns can become aware of the barbed wire traps, affecting their behavior and pathfinding.
- **Harm Mechanism:** integrated harm function that affects pawns upon interaction with the barbed wire.

## Coding Patterns and Conventions

- **Namespace Management:** Ensure all classes and methods are encapsulated within appropriate namespaces for modular development.
- **Method Naming Conventions:** Public methods follow PascalCase naming conventions, ensuring readability and consistency.
- **Access Modifiers:** Use private where appropriate to encapsulate data and methods not intended for external use.
- **Inheritance:** Utilize base/abstract classes for shared functionality, reducing redundancy and enhancing code maintainability.

## XML Integration

- **XML Data Handling:** Utilize XML files to manage configuration and balancing aspects of the barbed wire, such as damage values and costs.
- **Integration Points:** Ensure XML values are correctly scraped and used within classes, allowing for easy tweaks and modifications without direct code changes.

## Harmony Patching

- **Harmony Usage:** Employ Harmony patches to alter or extend vanilla RimWorld behavior without modifying the base game code directly.
- **Patch Types:** Utilize prefix or postfix patches for methods that need to observe or modify behavior before or after the original method execution.
- **Compatibility:** Ensure patches are designed to be compatible with other mods and are fail-safe by checking for null references and other potential issues.

## Suggestions for Copilot

- **Auto-Generate Methods:** As you develop, use Copilot to suggest method implementations based on comments and docstrings.
- **Pattern Recognition:** Leverage Copilot’s ability to recognize patterns in existing code to suggest similar implementations for new features.
- **XML and C# Integration:** Use Copilot to assist in writing methods that handle XML data, ensuring values are extracted and used properly within C# classes.
- **Harmony Patch Suggestions:** Allow Copilot to suggest possible Harmony patch points in RimWorld base methods, aiding in efficient mod behavior modification.
- **Error Checking:** Make use of Copilot's suggestions to improve error handling and to check for potential null references or exceptions in methods.

By utilizing these guidelines and Copilot’s capabilities, this mod will be both robust and flexible, offering a seamless integration of new defensive structures into RimWorld.
