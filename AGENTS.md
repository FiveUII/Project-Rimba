# Workspace Rules: Game Project

Guidelines khusus untuk pengembangan game di workspace ini. Prioritaskan performa runtime dan efisiensi penulisan kode.

## 1. Game Architecture & Conventions
- **Pemisahan Tanggung Jawab**: Pisahkan Core Gameplay logic, UI, Audio, dan Data/Save state.
- **Event-Driven vs Polling**: Gunakan C# Actions / Events atau Observer pattern untuk komunikasi antar-sistem. Hindari memeriksa kondisi terus-menerus di `Update()` jika bisa dipicu oleh event.
- **FixedUpdate vs Update**:
  - `FixedUpdate()` hanya untuk kalkulasi physics dan pergerakan Rigidbody / Rigidbody2D.
  - `Update()` untuk input polling dan non-physics logic.
- **Memory & Garbage Collection**:
  - Jangan gunakan `Instantiate` dan `Destroy` berulang kali saat gameplay aktif; gunakan **Object Pooling**.
  - Hindari alokasi array/string sementara di dalam loop `Update()` untuk mencegah GC spike / framerate stuttering.

## 2. Minimalist Coding (Ponytail applied to Game Dev)
- Sebelum membuat MonoBehaviour baru, periksa apakah fungsi cukup diletakkan pada komponen yang sudah ada.
- Gunakan komponen bawaan engine (misal: Unity CharacterController, AudioSource, Tilemap) sebelum menulis sistem kustom dari nol.
- Perubahan kode harus sekecil mungkin tanpa merusak scene hierarki atau prefab bindings.

## 3. Communication
- Langsung ke inti teknis (Caveman style). Jangan sertakan penjelasan teori berbelit kecuali diminta.
