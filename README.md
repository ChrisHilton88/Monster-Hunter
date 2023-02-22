# Monster-Hunter
GDHQ Framework project in Unity - FPS with waves of monsters spawning that you have to kill before they reach a target point

*** WORK IN PROGRESS *** 3D Action Shooter. Many different waves of monsters will spawn at a start point and attempt to reach the end point. Make sure you shoot them all!

This project was designed under the GDHQ Framework projects. This project is designed to showcase the use of AI and the NavMesh system in Unity. It uses the Universal Render Pipeline (URP).

It also incorporates:

Enemy/AI: - Finite State Machines (FSM) for the AI utilising Unity's Animator component (and Blend Trees). - Spawn Manager with a balanced wave system using Scriptable Objects.

Animations - Range of Animations for the Enemies, dependent on which state from their FSM they are currently in.

Player - Shooting system with a crosshair on screen. - Zoom in through sniper lens feature using Cinemachine. - Dynamic Reload system - You choose when you want to reload. Has a minimum and maximum ammo count. - Layer mask system that identifies whether it hits an Enemy, IDestroyable object or the Environment. - Player movement contained to the Shooting Box. - Cursor is hidden on the screen.

Environment - Universal Render Pipeline (URP). - Sci-Fi facility setting. - Utilises the Unity NavMesh system - Lighting features including: - Start and End spawn points for the Enemies.

Installed Packages - AI Navigation (Experimental) - "com.unity.ai.navigation" - Test Framework - Profiler - TextMeshPro (TMP) - Cinemachine - Timeline - New Input System - Universal Render Pipeline (URP)
