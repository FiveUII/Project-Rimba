---
name: game-architecture
description: Petunjuk standar merancang hierarki game, State Machine, GameManager, dan Object Pooling. Gunakan saat membuat sistem gameplay baru atau menstrukturkan alur game.
---

# Game Architecture & Loop Guide

Panduan merancang sistem game yang terstruktur, modular, dan ringan performa.

## 1. Struktur Inti (Core Singletons / Managers)
- **GameManager**: Mengatur state level/game (MainMenu, Playing, Paused, GameOver).
- **Service Locator / Events**: Hindari dependensi langsung antar objek berbeda layer (misal: Enemy tidak boleh mengakses langsung UI Canvas; gunakan event `OnEnemyDied`).

## 2. Object Pooling Workflow
Untuk peluru, efek partikel, atau musuh yang sering spawn:
1. Buat pool container di awal scene (`Awake`/`Start`).
2. Ambil objek dengan `Get()` dan aktifkan `gameObject.SetActive(true)`.
3. Kembalikan objek ke pool dengan `Release()` saat mati/keluar batas layar, bukan `Destroy()`.

## 3. Input & Cross-Platform
- Pisahkan pembacaan input dari aksi eksekusi karakter.
- Parameter pergerakan selalu dikalikan `Time.deltaTime` (atau `Time.fixedDeltaTime` pada physics).
