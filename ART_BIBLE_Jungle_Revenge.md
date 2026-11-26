# ART BIBLE — *Jungle Revenge* (LOCKED)

> **Status:** Terkunci sebagai acuan tunggal seluruh aset visual *Jungle Revenge* (Target rilis v1 `< 1 bulan`, produksi solodev). Jangan mencampur gaya vektor HD, ilustrasi AI detail langsung, atau kotak polos pada aset final.

---

## 1. Identitas Visual & Scope v1
* **Judul Gim:** *Jungle Revenge* (Unity 6, Universal 2D / URP)
* **Gaya Visual:** **Atmospheric Low-Res Hi-Bit Pixel Art** (inspirasi utama: *Rusted Moss*, *Celeste*, *Kingdom: Two Crowns*).
* **Prinsip Anti-"AI Slop":** Semua aset karakter, prop, dan latar digambar datar (*flat cel-shaded lineless*) lalu dikonversi ke piksel `1x` sejati melalui pipeline `hibit` (`agent-sprite-forge`) dengan palet terkunci dan alpha biner. Kedalaman dibangun lewat **parallax 3 layer + kabut + URP 2D Lighting**, bukan shading ramai.
* **Scope Rilis v1 (15–20 Menit Playtime):**
  1. Cutscene Pembuka (6 panel statis *widescreen* `480x270`)
  2. **Stage 1-1:** Kanopi Rimba & Ekspansi Sawit (bebaskan **Ular Piton**)
  3. **Stage 1-2:** Kamp Pembalakan & Lahan Gambut Membara (`BurningBranch`)
  4. **Boss Arena:** Puncak Kamp Pembalakan / Pembakaran Hutan

---

## 2. Standar Resolusi, Kamera & Import Unity (Wajib Seragam)
* **Kanvas Asli (`1x`):** `480 x 270` px (`16:9`, integer `4x` = `1920 x 1080`).
* **Grid Tile:** `16 x 16` px (`30 x 16.875` tile terlihat per layar).
* **Pengaturan Unity Wajib:**
  * `Pixels Per Unit (PPU)`: **`16`** untuk seluruh sprite & tilemap gameplay.
  * `Filter Mode`: **`Point (no filter)`**.
  * `Compression`: **`None`**.
  * `Camera`: **`PixelPerfectCamera`** (`480 x 270` reference resolution).
  * `Alpha`: **Binary** (`0` atau `255`, tanpa *anti-aliasing* atau transparansi setengah di sprite karakter/prop).

### Skala Aset Terkunci (Ukuran Piksel `1x`)
| Kategori Aset | Tinggi / Ukuran `1x` | Catatan & Acuan |
| :--- | :--- | :--- |
| **Hero — Orangutan Kalimantan *(LOCKED)*** | **`48 px`** tinggi bodi (`53 x 50` px total, `anchor: [22, 49]`) | Master: `art_forge/orangutan-master/master.json`; Palet: `art_forge/orangutan-hibit-48/palette.json` (`16` warna + outline `#360800`). |
| **Mob (Penebang Indonesia / Pelempar)** | `50–54 px` tinggi (`~34x52`, berdiri tegak ramping vs Hero bungkuk lebar) | Lebih tinggi sedikit dari orangutan bungkuk untuk kontras siluet & tetap nyaman di-*pogo*; `16` warna + `--outline auto`. |
| **NPC Hewan (Ular Piton) & Kandang** | Piton `24–28 px`; Kandang `48–56 px` (`3–3.5` tile) | Kandang punya 3 state (`closed`, `cracked`, `open`). |
| **Boss Latar 1** | `64–76 px` tinggi (`~1.4x–1.6x` Hero, manusia + peralatan berat/tangki; `80–96 px` khusus bos alat berat) | Siluet mengintimidasi dengan *telegraph* warna terang yang jelas. |
| **Proyektil & Item (Kapak, Pisang)** | `14–18 px` | Kontras tinggi agar terbaca di udara. |
| **Environment (`Tilemap` & Prop)** | Tile `16 x 16 px`; Duri Sawit `16 x 16 px`; Pohon `96–128 px` tinggi; Gubuk `80–96 px` lebar | Dahan grapple dipisah agar bisa di-highlight runtime tanpa menyinari seluruh pohon. |

---

## 3. Palet Warna Terkunci (Hex Exact)

### A. Hero: Orangutan Kalimantan (`17` Warna — Verbatim `palette.json`)
* **Outline Otomatis (`1 px`):** `#360800`
* **Bulu & Bayangan Keunguan (16 warna):** `#481d3a`, `#591f28`, `#7d3735`, `#973617`, `#9d3a1a`, `#aa481d`, `#955158`, `#c25d28`, `#b06960`, `#d56a28`, `#d87437`, `#d9936b`, `#f7a253`, `#f4b481`, `#f9bc89`, `#fde0bf`

### B. Stage 1-1: Rimba Berlumut (*Rusted Moss* Atmosphere)
* **Langit & Kabut Parallax (jauh → dekat):** `#1A262B`, `#273A3E`, `#3B5452`, `#5A786E`, `#84A390`
* **Lumut & Kanopi Hutan:** `#1E3028`, `#2D4A38`, `#466E4B`, `#6C995C`
* **Tanah, Akar & Kayu Lapuk:** `#291C1A`, `#472E26`, `#6E4434`, `#945E44`

### C. Stage 1-2 & Boss: Gambut Membara (*Karhutla*)
* **Langit, Asap & Siluet Hangus:** `#1D1419`, `#4A2324`, `#8F3E2A`, `#D97838`
* **Bara Api & Cahaya:** `#FFAE42` + tint cahaya oranye hangat dari **URP 2D Light** (aset pohon/tanah tetap sama agar hemat produksi).

### D. Warna Sinyal Gameplay (Wajib Kontras di Atas Latar Gelap)
* **Highlight Dahan Grapple:** `#A2CC72`, `#D6F29B`
* **Bahaya / Duri Sawit / Ikat Kepala Musuh / Telegraph:** `#B52F35`, `#E64E4E`, `#FFD875`
* **Skill Ular Piton / Heal Glow:** `#3E8C4A`, `#7BC468`, `#CFE892`
* **Status Terbakar (`Burn` VFX):** `#FF6B2B`, `#FFAE42` + partikel bara & asap pada tubuh Orangutan.

### E. Desain Karakter Musuh Indonesia & Variasi Baju *(LOCKED)*
Setiap tipe musuh punya **siluet unik** agar terbaca instan dalam `0.1` detik, lalu divariasikan memakai **3 *palette swap*** (`palette_tool.py variants / luts`, 0 animasi tambahan):
1. **`AxeLogger` (Penebang Kapak — Mob Dasar):** Kaos oblong + **topi pet gelap** + celana kerja + kapak baja.
2. **`TorchLogger` (Penebang Obor / Pelempar — Ranged):** Kaos gelap + **tas pinggang (*waist bag*)** + obor / proyektil api.
3. **`ChainsawLogger` (Penebang Gergaji Mesin — Elite):** Baju lengan panjang + **kupluk/penutup kepala gelap** + **sepatu bot karet tinggi** + gergaji mesin besar (`RevUp` → `Rush` → `Stall`).
4. **Boss Latar 1 — *Si Pembakar Hutan (Mandor Kaleng Api)* (`64–70 px`):** Kain penutup hidung/mulut, **sabuk penuh selipan kaleng semprot (aerosol)** di dada/pinggang, tangan kiri memegang **korek api menyala** (`#FFD875`), tangan kanan memegang **kaleng semprot**, sepatu bot karet.

* **3 Variasi Palet Baju (Dipakai Acak per Spawn):**
  * **Varian 1:** Atasan *olive/khaki* pudar + celana merah marun + bot hitam.
  * **Varian 2:** Kaos abu-hitam lusuh + celana gelap + tas pinggang.
  * **Varian 3:** Kaos biru pudar / krem berlumpur + celana training gelap berlumpur.
  * **Aturan Kontras:** Dilarang memberi kaos oranye terang pada musuh agar tidak bentrok secara visual dengan bulu **Orangutan (`#E0782B` / `#F5AC56`)**.

---

## 4. Pipeline Produksi Aset (`agent-sprite-forge`)
1. **Karakter, Mob, NPC & Boss (`generate2dsprite` + `video2dsprite`):**
   * Buat prompt `master_still.py prompt --finish hibit --key magenta` (gaya flat *lineless cel-shaded*, maks 3 nada per material, bayangan dingin keunguan, highlight hangat).
   * Generate via `host-image_gen`, review visual, edit seperlunya dengan `master_still.py edit`, lalu kunci dengan `master_still.py approve`.
   * Konversi ke piksel via `finish_frames.py hibit --target-height <H> --colors 16 --outline auto` (atau `--scale-ref` + `--palette <karakter>/palette.json` untuk aksi lanjutan/animasi).
2. **Background Parallax & 6 Panel Cutscene Pembuka (`generate2dmap`):**
   * Lukis datar *lineless* → proses dengan `hibit_plate.py plate` (`480 x 270`, `14` warna + *shade map*) supaya gaya cutscene pembuka 100% menyatu dengan gameplay.
3. **FX & Partikel (`fx_pixelfx.py` / `hibit_fx.py`):**
   * Efek serangan, percikan kapak, bara, dan ledakan dikonversi melalui `hibit_fx.py pixelize --snap <palette.json>` (atau `fx_pixelfx.py` dengan atribusi `CREDITS.txt`).
4. **Atmosfer In-Engine (Unity URP 2D):**
   * Tambah kedalaman hanya di Unity: *URP 2D Global Light*, *Point Light* untuk lentera gubuk & api, partikel piksel `1x1`–`2x2` px (spora rimba / bara api), *hit-stop* (`0.04s`), dan *camera shake* tipis.

---

## 5. Organisasi Folder, Penamaan & Gerbang QA
* **Workspace Kerja Mentah (di luar Unity):** `art_forge/` (menyimpan `prompt/`, `inbox/`, `master/`, dan output mentah `hibit` + laporan QA).
* **Aset Final Siap Pakai di Unity:** `Project Rimba/Assets/Project/Art/`
  * `Characters/` — sprite sheet & palette final karakter/musuh/NPC
  * `Environment/` — tileset, pohon+dahan, gubuk, duri, prop
  * `Backgrounds/` — layer parallax & panel cutscene
  * `Animations/` — `.anim` dan `.controller`
  * `Materials/`, `Physics/`, `UI/`
* **Konvensi Penamaan File Final:**
  * `spr_<entitas>_<aksi>.png` (contoh: `spr_orangutan_idle.png`, `spr_lumberjack_sheet.png`)
  * `tile_<bioma>_<jenis>.png` (contoh: `tile_rimba_ground.png`)
  * `bg_<stage>_<layer>.png` (contoh: `bg_stage1_sky.png`, `bg_stage1_canopy.png`)
  * `cutscene_<01..06>.png`
* **Syarat Wajib Sebelum Masuk `Project Rimba/Assets/`:**
  1. `alphaBinary: true`, `offPaletteColors: 0`, dan `opaque_key_px: 0`.
  2. Terbaca jelas di atas latar gelap `#1A262B` pada zoom `1x` maupun `4x` (`NEAREST`).
  3. Tidak ada aset lama beresolusi HD/AI campur yang tersisa di scene final.
