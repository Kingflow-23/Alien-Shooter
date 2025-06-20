# First-Person Shooter Game

A Unity-based first-person shooter that blends fast-paced combat with interactive systems and immersive effects.

---

## 🎯 Overview

This project features both projectile and hitscan mechanics (projectiles are currently used exclusively), with support for shotguns and pellet spread. Players can switch among six weapons—sniper, regular gun, shotgun, rifle1, rifle2, and rifle3—via number keys or the scroll wheel. Each weapon carries its own data (fire rate, damage, projectile force) and a unique firing sound.

Enemies spawn dynamically around the player within a set radius, up to a configurable limit. There are four distinct enemy types, each varying in speed, size, health, damage, and score value. A real-time kill counter tracks progress. Once the player reaches the target kill count (default 20), a hidden goal object is activated at a random location. The game is won by destroying or reaching this goal.

Health pickups spawn randomly to help the player recover. Bullet impacts generate a temporary visual effect—originally envisioned as a black hole—that disappears after a few seconds when hitting enemies, the ground, or the goal. Ambient snow effects and a clean overlay-based UI display health, win/lose messages, and restart controls. Audio is managed by a centralized `AudioManager` singleton, handling background music, weapon SFX, and monster attack noises, with the main theme playing at startup.

---

## 🚀 Features

- **Weapon System**  
  - Six weapons with individual stats and audio  
  - Projectile and hitscan support (projectile used)  
  - Shotgun spread logic  

- **Enemy Spawning**  
  - Four enemy types with distinct behaviors  
  - Configurable spawn radius and maximum count  
  - Score awarded per kill  

- **Objective & Progression**  
  - Kill counter UI  
  - Hidden goal spawns at configurable kill threshold  
  - Win by reaching or destroying the goal  

- **Health & Pickups**  
  - Randomly placed health packs  
  - Recover mid-battle  

- **Visual Effects**  
  - Temporary impact effect on bullet collision  
  - Ambient snow particle system  

- **Audio**  
  - Singleton `AudioManager`  
  - Background music and main theme  
  - Weapon-specific firing sounds  
  - Monster attack SFX  

- **UI & UX**  
  - Overlay canvas for health bar, score, and messages  
  - Restart game button  
  - Animated win/lose prompts  

---

## 🎮 Controls

- **Move**: `W` / `A` / `S` / `D`  
- **Look**: Mouse  
- **Fire**: Left Mouse Button  
- **Aim**: Right Mouse Button  
- **Switch Weapon**: Number Keys `1–6` or Mouse Scroll Wheel  
- **Jump**: `Space`  
- **Sprint**: `Left Shift`  

---

## Demo


---

## 🛠 Installation & Setup

1. **Clone** this repository.  
2. **Open** the project in Unity (version 2021.3+ recommended).  
3. **Assign** AudioClips, prefabs, and references in the Inspector for:  
   - `GameManager` (enemy prefabs, health pickups, goal prefab, UI references)  
   - `AudioManager` (music and SFX arrays)  
   - Each weapon prefab (assign `WeaponData`, projectile prefab, shoot point).  
4. **Tag** your enemies as `Monster` and the goal as `Goal`; tag the ground as `Ground`.  
5. **Run** the scene to play!

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to fork the repo and submit a pull request.

---

## 📜 License

This project is licensed under the MIT License. See `LICENSE` for details.

---