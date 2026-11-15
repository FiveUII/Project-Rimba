# DOKUMEN DESAIN GIM (GDD): PROJECT RIMBA

**Informasi Proyek & Tim Pengembang**
* **Judul Gim (Working Title):** Project Rimba
* **Anggota Tim (3 Orang):**
  1. **[Nama Kamu]** — *Game Designer & Core Gameplay Programmer*
  2. **[Nama Anggota 2]** — *AI / System Programmer & Level Designer*
  3. **[Nama Anggota 3]** — *2D Environment/Character Artist & UI/Audio Integrator*
* **Target Platform:** PC (Windows / macOS) — *Mouse & Keyboard Control*
* **Game Engine:** Unity 6 (Universal 2D / URP)

---

## BAGIAN 1: CONCEPT DOCUMENT (High-Level Vision)

### 1.1 High Concept Statement
> *"Project Rimba adalah gim 2D Action-Platformer yang menggabungkan pertarungan presisi dan eksplorasi atmosferik ala **Hollow Knight** dengan mekanik ayunan kanopi (**Grapple Swing / Brachiation**) di tengah krisis perusakan hutan dan tambang Kalimantan."*

### 1.2 Deskripsi Singkat (Description)
Pemain mengendalikan seekor Orang Utan Kalimantan yang harus bertahan hidup dan melawan ekspansi perusak lingkungan—mulai dari pembalakan liar, pembakaran hutan gambut (*karhutla*), hingga kawah tambang batu bara terbuka. Pergerakan dasar menggunakan kontrol *platformer* presisi (`A/D`, lompat, *dash*) yang dipadukan dengan cengkeraman ayunan lengan (`Grapple Swing`) ke dahan/pipa serta serangan melee 4 arah (*termasuk pogo*) menggunakan mouse. Di sepanjang perjalanan, pemain membebaskan satwa endemik Kalimantan untuk membuka *skill* bantuan baru.

### 1.3 Genre, Spin, & Flavor
* **Genre:** 2D Action-Platformer / Metroidvania-lite.
* **Spin (Keunikan Mekanik):** Kombinasi *combat* responsif ala *Hollow Knight* (pukulan 4 arah + *pogo* + *dash reset*) dengan navigasi vertikal berbasis ayunan lengan orang utan (`Grapple Swing`) pada dahan dan struktur tambang.
* **Flavor (Nuansa & Tema):** Atmosferik, intens, dan sarat pesan ekologis nyata tentang deforestasi serta pertambangan di Kalimantan.

### 1.4 Fitur Utama (Key Features)
1. **Tight Combat & Canopy Grapple Swing:** Gerak, lompat, *dash*, pukulan 4 arah (*pogo* di atas musuh/duri me-reset *dash*), serta ayunan lengan sebatas jangkauan tangan ke dahan `Grabbable`.
2. **Endemic Animal Assist Skills:** Bebaskan 4 satwa Kalimantan (Ular Piton, Buaya Muara, Trenggiling, dan Burung Enggang) untuk memperluas kemampuan navigasi dan pertarungan.
3. **2 Latar Kontras (6 Stage / Scope Terukur):** Perjalanan dari Hutan Hujan Kalimantan yang dibakar habis menuju Kawah Tambang Batu Bara Terbuka.
4. **Telegraphed Boss Fights:** Pertarungan *miniboss* dan *final boss* yang menuntut ketepatan membaca aba-aba (*telegraph*) dan kelincahan berpindah posisi.

---

## BAGIAN 2: GAME DESIGN DOCUMENT (Functional Spec - "WHAT")

### 2.1 Cerita & Narasi (Story)
Rumah kanopi tempat tinggal sang Orang Utan mulai runtuh ketika kelompok penebang liar dan pembuka lahan sawit merangsek masuk ke hutan dalam. Saat sang Orang Utan berusaha mempertahankan wilayahnya dan membebaskan satwa-satwa lain yang tertangkap, hutan gambut tersebut dibakar habis untuk membuka lahan secara instan. Ketika asap kebakaran tersibak di batas hutan yang hangus, terlihat alasan utama di balik kehancuran tersebut: sebuah operasi tambang batu bara terbuka berskala raksasa yang menggerus perut bumi Kalimantan.

### 2.2 Mekanik Permainan (Game Mechanics)

#### A. Skema Kontrol Pergerakan & Pertarungan (Keyboard + Mouse)
| Input Pemain | Fungsi Pergerakan (Navigasi) | Fungsi Pertarungan (Combat) |
| :--- | :--- | :--- |
| **`A` / `D` (atau Panah)** | Jalan & kendali udara presisi; memompa ayunan saat bergelantungan. | Mengatur jarak serang dan menghindari *telegraph* musuh. |
| **`Space`** | **Variable Jump** (tekan singkat = lompat pendek, tahan = lompat penuh) + *coyote time* & *jump buffer*. | Melompati serangan bawah/proyektil. |
| **`Left Shift`** | **Dash:** Meluncur cepat ke arah hadap (*cooldown* `0.6` detik; langsung reset saat *Pogo* berhasil). | Menghindar menembus celah serangan bos. |
| **Klik Kiri (+ Arah Mouse)** | **Pogo Bounce:** Memukul ke bawah di udara pada musuh atau duri memantulkan badan ke atas. | **4-Way Melee Slash:** Pukulan jarak dekat ke arah kursor (atas, bawah, kiri, kanan). |
| **Tahan Klik Kanan** | **Grapple Swing:** Mengait dahan `Grabbable` terdekat kursor dalam jangkauan lengan (`3.8` unit); lepas untuk meluncur. | Menjaga posisi di atas arena atau mengayun cepat melewati bahaya. |
| **Tahan `E` / Tengah Mouse** | — | **High-Risk Heal:** Menahan `1.5` detik saat menapak atau bergelantungan untuk memulihkan **1 Segmen HP** (biaya `40` Adrenalin). |

#### B. Sistem HP, Hurtbox, & Adrenalin
* **Hurtbox (Badan Orang Utan):** Jika terkena serangan musuh atau duri (`Hazard`), mengurangi **1 Segmen HP** dan memberikan waktu kebal (*Invincibility Frames*) selama `1.2` detik.
* **HP Bar Berbasis Segmen:** Pemain memiliki **5 Segmen HP** (5 Ikon Daun Rimba).
* **Bar Adrenalin (Mirip Sistem *Soul*):**
  * Memukul musuh dengan serangan melee mengisi **Bar Adrenalin** (`+20` per pukulan, maks `100`).
  * Menguras `40` Adrenalin untuk memulihkan **1 Segmen HP** lewat *High-Risk Heal*.

#### C. Sistem Skill Hewan (NPC Assist)
Dibuka dengan menghancurkan kandang hewan di dalam *stage*:
1. **Ular Piton (Pasif / Upgrade Grapple):** Memperpanjang jangkauan **Grapple Swing** hingga $2\times$ lipat (`3.8` → `7.6` unit) untuk menggapai dahan jauh.
2. **Buaya Muara (`S` + Klik Kiri di Udara / Tombol Skill):** **Jaw Crush (Ground Slam)** — menghujam ke bawah untuk menjebol lantai lapuk atau perisai atas bos.
3. **Trenggiling (`Ctrl` / Tombol Skill):** **Iron Scale Curl** — meringkuk menjadi perisai lapis baja tahan ledakan dinamit dan memantulkan proyektil (menguras Adrenalin).
4. **Burung Enggang (Otomatis `Space` di Udara):** **Canopy Updraft (Double Jump)** — lompatan kedua berbasis hembusan angin di udara.

---

### 2.3 Spesifikasi Level & Musuh (Level Specs)

#### LATAR 1: HUTAN HUJAN KALIMANTAN
* **Stage 1-1: Kanopi Rimba & Ekspansi Sawit**
  * *NPC Hewan:* **Ular Piton**.
  * *Musuh:* **Penebang Kapak** (patroli + *telegraph* kuning sebelum menebas) & **Pelempar Bibit Sawit** (proyektil parabolik berduri).
* **Stage 1-2: Kamp Pembalakan & Sungai Gelondongan Kayu**
  * *NPC Hewan:* **Buaya Muara**.
  * *Miniboss:* **Jagal Gergaji Mesin** — kebal dari depan; pancing menabrak pohon hingga tersangkut `3` detik lalu hantam dengan **Buaya Muara** / *Pogo*.
* **Stage 1-3: Lahan Gambut Membara**
  * *Mekanik Lingkungan:* **Dahan Terbakar** (`BurningBranch` — maksimal dicengkeram `1.5` detik sebelum memicu *Grip Break* dan melukai pemain).
  * *Musuh:* **Anak Buah Tongkat Api**.
  * *Final Boss Latar 1:* **Si Pembakar Hutan** (*telegraph* korek api → semburan aerosol api).

#### LATAR 2: TAMBANG TERBUKA KALIMANTAN
* **Stage 2-1: Lereng Overburden & Tebing Batu Bara**
  * *NPC Hewan:* **Trenggiling**.
  * *Musuh:* **Pekerja Beliung** (helm besi kebal *pogo* biasa, menjatuhkan batu).
* **Stage 2-2: Sabuk Konveyor & Zona Peledakan**
  * *NPC Hewan:* **Burung Enggang**.
  * *Miniboss:* **Mandor Dinamit** (tahan ledakan dengan **Trenggiling**, kejar ke menara dengan **Burung Enggang**).
* **Stage 2-3: Dasar Kawah Asam & Jantung Tambang**
  * *Final Boss Latar 2:* **Raksasa Alat Berat (Bucket-Wheel Excavator)** — arena panjatan raksasa yang menggabungkan ke-4 *skill* hewan.

---

### 2.4 User Interface (UI), Visual, & Audio
* **HUD Minimalis (Pojok Kiri Atas):** 5 Ikon Daun HP + Bar Adrenalin + Bar Progress Heal + **Reticle Kursor Dinamis** (Hijau = masuk jangkauan `Grabbable`, Kuning = sedang mengayun).
* **Audio:** Fokus pada **Audio Telegraph** (klik korek api, deru gergaji, desis sumbu dinamit).

---

## BAGIAN 3: TECHNICAL SPECIFICATION RINGKAS ("HOW")

1. **Arsitektur Karakter (`OrangutanController`):**
   * **Torso (`Rigidbody2D`):** `Mass = 5.0`, `Gravity Scale = 3.0`, `Freeze Rotation Z = True`, `Collision Detection = Continuous`.
   * **Gerak & Lompat:** `linearVelocity` deterministik dengan *coyote time* (`0.12s`), *jump buffer* (`0.12s`), dan *variable jump cut*.
   * **Grapple Swing (`DistanceJoint2D`):** `maxDistanceOnly = true`, mengunci jarak ke titik dahan dalam radius `grappleRange` (`3.8` unit) + `LineRenderer` sebagai visual lengan.
   * **Melee & Pogo:** `Physics2D.OverlapCircle` non-alokasi (`ContactFilter2D`) 4 arah; serangan bawah ke `Enemy` atau `Hazard` memicu *pogo bounce* dan me-reset *dash*.
2. **Pemisahan Layer Physics 2D:**
   * `PlayerBody`, `Grabbable`, `Ground`, `Enemy`.

---

## BAGIAN 4: PEMBAGIAN TUGAS TIM (3 ORANG)

### 👤 Orang 1 (Kamu): Lead Game Designer & Core Gameplay Programmer
* Finalisasi GDD, *tuning* `OrangutanController`, implementasi 4 *Skill* Hewan, *HUD*, dan *Stage Flow*.

### 👤 Orang 2: AI / System Programmer & Level Designer
* State Machine musuh & *boss*, objek lingkungan (`BurningBranch`, `Hazard`, pohon tumbang, kandang), dan *greybox* level.

### 👤 Orang 3: 2D Artist, Animator & UI/Audio Integrator
* *Sprite* & animasi Orang Utan, 4 Hewan NPC, Musuh & Boss, *Tileset/Parallax*, dan SFX *telegraph*.
