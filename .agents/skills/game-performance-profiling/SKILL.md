---
name: game-performance-profiling
description: Panduan mendiagnosis masalah FPS drop, lag, memory leak, dan draw call berlebih pada game. Gunakan saat game terasa patah-patah atau berat.
---

# Game Performance & Profiling Checklist

Langkah sistematis mengidentifikasi dan memangkas beban komputasi game.

## 1. Cek Sumber Lag Utama
- **CPU Bound**: Terlalu banyak loop di `Update()`, pathfinding NavMesh terlalu sering dihitung ulang, atau physics step terlalu rapat.
- **GPU Bound**: Resolusi tekstur terlalu besar, overdraw material transparan berlapis, atau unoptimized post-processing volume.
- **GC Spikes**: Alokasi `new` di dalam loop per frame (list, string concats, coroutines berulang).

## 2. Langkah Solusi Hemat Kode
1. Ubah teks concatenation di UI menjadi event-only (hanya update text saat nilai skor/darah benar-benar berubah).
2. Gunakan Sprite Atlas untuk elemen UI/2D agar draw calls ter-batching otomatis.
3. Batasi raycast layer masks hanya pada layer objek target, bukan `Physics.Raycast` tanpa layer mask.
