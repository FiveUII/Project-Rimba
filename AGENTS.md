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

## 4. Sprite & 2D Art Generation (Mandatory)
- Setiap kali user meminta membuat, mengubah, atau me-remodel sprite 2D, karakter, prop, FX, animasi, tile, atau background/plate, WAJIB baca dan ikuti skill `agent-sprite-forge` di `.agents/skills/` (`generate2dsprite`, `video2dsprite`, `generate2dmap`, `generate2dmedia`, serta `codeart2d` hanya jika diminta eksplisit atau tidak ada rute model gambar/video).
- Untuk game pixel art (*Jungle Revenge* / gaya *Rusted Moss*), gunakan pipeline `hibit` (`master_still.py prompt --finish hibit` -> buat master flat lineless -> `master_still.py approve` -> `video2dsprite/scripts/finish_frames.py hibit`) atau `pixel_look.py` saat menyamakan dengan referensi piksel; simpan proses kerja di `art_forge/` (di luar `Project Rimba/Assets/`) dan salin hasil final yang sudah disetujui ke `Project Rimba/Assets/Project/Art/`.
- Selalu patuhi standar visual terkunci di `ART_BIBLE_Jungle_Revenge.md` (Hero `36 px`, `16` PPU, kanvas `480x270`, `Filter Mode: Point`, palet & skala terkunci).
- Selalu laporkan provenance (`route`, model, angka QA / peringatan WARN atau FAIL secara verbatim).
