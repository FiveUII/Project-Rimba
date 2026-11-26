# MAP & STORY DESIGN — *Jungle Revenge* (Latar 1 v1, 15–20 Menit)

> **Acuan Visual:** [ART_BIBLE_Jungle_Revenge.md](ART_BIBLE_Jungle_Revenge.md) (`480x270`, `16` PPU, grid `16x16` px = `30 x 16.875` tile per layar kamera).

---

## 1. Alur Lengkap v1 & Opening Cutscene
`Main Menu` → **`Opening Cutscene` (6 Panel)** → **`Stage 1-1`** → **`Stage 1-2`** → **`Boss Arena`** → **`Victory / Teaser`**

### Opening Cutscene (6 Panel Statis `480x270` `hibit_plate`, ~25–30 dtk, `Skip [Space]`)
1. **Panel 1 (Satelit):** Peta Pulau Kalimantan dilihat dari atas dengan kepulan asap kebakaran hutan oranye-abu (*fade-in* lambat).
2. **Panel 2 (Pelarian):** Siluet Orangutan & keluarganya berlari/mengayun di tengah kanopi yang mulai dikepung api.
3. **Panel 3 (Tragedi):** Keluarga terkepung api; protagonis berbalik hendak menolong, lalu **batang pohon terbakar tumbang menghantam & memisahkan jalan** → layar gelap (*blackout* + SFX dentuman).
4. **Panel 4 (Hening):** Terbangun di tanah berabu; menatap **sisa sarang hangus** keluarganya dalam kesunyian.
5. **Panel 5 (Pemicu):** Melangkah ke batas hutan yang masih hijau; terlihat siluet penebang menebang pohon dekat **jerigen bahan bakar & obor**.
6. **Panel 6 (Tekad):** *Close-up* mata Orangutan dengan pantulan bara api → langsung transisi masuk **Stage 1-1**.

---

## 2. Aturan Bahaya Lingkungan & Status `Burn`
* **Jatuh ke Jurang Tebangan (`Pitfall`):**
  * Mengurangi **`1` Segmen HP** lalu me-respawn pemain di titik pijakan aman terakhir (`SafeGroundTracker`).
* **Terkena Api Gambut / `BurningBranch` Overhold (`Burn` DoT):**
  * Memicu status **`Burn` (Terbakar)** pada Orangutan: mengurangi **`1` Segmen HP setiap `4.0` detik** secara berkelanjutan hingga HP habis **kecuali dipadamkan dengan melakukan Heal (tahan `E` / makan pisang)**.
  * **Aturan Anti Soft-Lock:** Setiap area gambut/api wajib menyediakan **musuh ringan atau sarang/objek pengisi Adrenalin** di pulau pijakan terdekat agar pemain yang kehabisan Adrenalin tetap bisa mengisi bar (`40` Adrenalin) dan memadamkan api.

---

## 3. Blueprint Level Latar 1

### A. Stage 1-1: *Kanopi Terkoyak* (`~6–8 menit`, lebar `~160` tile / `~5.3` layar)
* **Struktur:** *Linear bercabang ringan* (jalur tanah bawah vs jalur kanopi atas) + **1 *short backtrack*** untuk membebaskan **Ular Piton**.
* **Zona A — Tepi Hutan Hangus (Tile `0–45`, Tutorial Organik):**
  * Gerak, lompat variabel, *pogo* melewati cluster duri sawit pertama, dan pertarungan dengan `2` `AxeLogger` di depan gubuk awal.
* **Zona B — Kanopi Ganda & *Short Backtrack* Piton (Tile `45–115`):**
  * Dari jalur bawah pemain melihat **Kandang Ular Piton** berada di ceruk kanopi atas-belakang yang terhalang dinding tinggi dari sisi bawah.
  * Pemain memilih melewati camp gubuk bawah (`AxeLogger` + `1` `TorchLogger`) atau ayunan dahan pendek atas (`grappleRange = 3.8`), mencapai menara platform di ujung Zona B, lalu **naik ke kanopi & berbalik ke kiri (`~25` tile)** untuk menghancurkan **Kandang Ular Piton** → memicu pop-up **Skill Ikatan Piton** (*jankauan Grapple `2x` lipat menjadi `7.6` unit*).
* **Zona C — Jurang Tebangan (Tile `115–160`, Ujian Skill Piton):**
  * Jurang lebar berturut-turut dengan dahan kanopi jauh yang **hanya bisa dicapai memakai Grapple `7.6` unit**, diselingi `1` pos jaga gubuk → *Checkpoint* & transisi menuju **Stage 1-2**.

### B. Stage 1-2: *Kamp Pembalakan & Gambut Membara* (`~7–8 menit`, lebar `~180` tile / `~6` layar)
* **Struktur:** *Linear bercabang ringan*, nuansa atmosfer bergeser ke langit/asap gambut oranye hangat.
* **Zona A — Kamp Gubuk Bertingkat (Tile `0–55`):**
  * Platforming antar atap gubuk panggung kayu; kombinasi `AxeLogger` di tanah dan `TorchLogger` yang melempar api dari atap gubuk.
* **Zona B — Zona Gergaji Mesin (Tile `55–105`, Pengenalan `ChainsawLogger`):**
  * `2` encounter terpisah melawan **`ChainsawLogger`**: *telegraph* tarik starter (`RevUp 0.9s`) → menerjang cepat (`Rush 0.9s`) → mesin tersendat (`Stall 1.5s`, jendela aman menghajar musuh). Tersedia dahan grapple di atas untuk *pogo* atau menghindar.
* **Zona C — Lahan Gambut Membara (Tile `105–155`):**
  * Rangkaian ayunan menggunakan **`BurningBranch`** (maks cengkeram `1.5s`) di atas permukaan tanah gambut berapi (memicu status **`Burn`**). Pulau pijakan tengah berisi `2` `AxeLogger` sebagai sumber Adrenalin.
* **Zona D — Gerbang Kamp (Tile `155–180`):**
  * Pertarungan penutup kecil (`1` `ChainsawLogger` + `1` `TorchLogger`) → area istirahat aman & *Checkpoint* sebelum arena Boss.

### C. Boss Arena: *Puncak Kamp — Si Pembakar Hutan (Mandor Kaleng Api)* (`~3–4 menit + retry`, lebar `~45` tile)
* **Tata Arena:** Lantai rata sepanjang `1.5x` layar (`~45` tile) dengan **3 dahan grapple** di ketinggian atas untuk rotasi posisi, kabur dari api, dan *pogo*.
* **Boss (`64–70 px`, `~650 HP` / `~26` pukul):** **Si Pembakar Hutan (*Mandor Kaleng Api*)** — memakai kain penutup hidung/mulut, sabuk penuh selipan kaleng semprot (aerosol), tangan kiri memegang korek api, tangan kanan memegang kaleng semprot.
* **Pola Serangan:**
  1. **Semburan Aerosol (*Aerosol Jet*):** Klik korek (`telegraph 0.7s`, *SFX `"KLEK!"`*) → semburan lidah api horizontal ke depan (memicu **`Burn`**). *Counter:* **Grapple** ke dahan atas atau lompat + **Pogo** di atas kepalanya.
  2. **Lempar Kaleng Panas (*Aerosol Bomb*):** Kocok kaleng (`0.6s`, *SFX `"KOCOK-KOCOK"`*) → lempar `1` kaleng (Fase 2: `2` kaleng) melengkung → mendesis `1.0s` lalu meledak menjadi genangan api gambut selama `4s` (memicu **`Burn`**). *Bonus Counter:* Kaleng di udara **bisa dipukul balik (*deflect*)** ke arah Boss agar langsung *stagger*.
  3. **Bakar Kanopi (*Canopy Sweep* — Fase 2 `< 50% HP`):** Angkat korek & kaleng ke atas (`telegraph 0.8s`) → menyemburkan api ke **1 dari 3 dahan grapple** menjadi **`BurningBranch`** selama `6` detik (memaksa pemain terus berpindah dahan).
  * **Jendela Balas Serang (*Ganti Kaleng* — `1.8s`):** Setiap `3` serangan kaleng habis (*SFX `"PSSSHT..."`*) → ia membuang kaleng kosong dan **mengambil + mengocok kaleng baru dari sabuknya (`1.8s`)**, memberi jendela aman untuk menghajar Boss atau **Heal (`E`)**.
* **Ending v1:** Si Pembakar tumbang, api di sektor rimba padam, siluet **Orangutan & Ular Piton** berdiri di atas kanopi yang mulai pulih dengan teaser kepulan asap tambang di kejauhan (*Latar 2*).

---

## 4. Urutan Eksekusi Produksi (< 1 Bulan)
1. **Aset Seragam (`agent-sprite-forge` `hibit`):**
   * Animasi **Orangutan** (`48 px`: Idle, Run, Jump, Swing, Attack 3 arah, Hurt, Heal).
   * `3` **Musuh Indonesia** (`50–54 px`: `AxeLogger`, `TorchLogger`, `ChainsawLogger` + `3` *palette swap* baju) & **Boss Pembakar Hutan** (`64–76 px`).
   * **Tilemap Rimba** (`16x16` Rule Tile), Pohon+Dahan Grapple, `BurningBranch`, Gubuk Panggung, Kandang+Piton, `3` Layer Parallax, & `6` Panel Cutscene (`hibit_plate`).
2. **Sistem Inti Unity:**
   * `PixelPerfectCamera` (`480x270`, `16` PPU), `SafeGroundTracker`, status **`Burn`** (+ padam lewat `Heal`), `ObjectPool` proyektil, AI `ChainsawLogger` & `TorchLogger`, dan state machine `ArsonistBoss`.
3. **Integrasi Level, UI & Audio:**
   * Greybox → dekorasi `Stage_1_1`, `Stage_1_2`, `Boss_1`, `OpeningCutscene`, `MainMenu` / Pause / Victory, plus `AudioManager` (*SFX telegraph* & BGM hutan/boss) lalu *playtest* & *build*.
