DROP TABLE IF EXISTS `biodata`;

CREATE TABLE `biodata` (
  `NIK` varchar(16) NOT NULL,
  `nama` varchar(100) DEFAULT NULL,
  `tempat_lahir` varchar(50) DEFAULT NULL,
  `tanggal_lahir` date DEFAULT NULL,
  `jenis_kelamin` TEXT CHECK (jenis_kelamin IN ('Laki-Laki','Perempuan')),
  `golongan_darah` varchar(5) DEFAULT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `agama` varchar(50) DEFAULT NULL,
  `status_perkawinan` TEXT CHECK (status_perkawinan IN ('Belum Menikah','Menikah','Cerai')),
  `pekerjaan` varchar(100) DEFAULT NULL,
  `kewarganegaraan` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`NIK`)
);

DROP TABLE IF EXISTS `sidik_jari`;

CREATE TABLE `sidik_jari` (

  `berkas_citra` text,
  `nama` varchar(100) DEFAULT NULL
);


INSERT INTO `sidik_jari` (`berkas_citra`, `nama`) VALUES
('../../../test/1__M_Left_index_finger.BMP', 'John Doe'),
('../../../test/1__M_Left_little_finger.BMP', 'John Doe'),
('../../../test/1__M_Left_middle_finger.BMP', 'John Doe'),
('../../../test/1__M_Left_ring_finger.BMP', 'John Doe'),
('../../../test/1__M_Left_thumb_finger.BMP', 'John Doe'),
('../../../test/1__M_Right_index_finger.BMP', 'John Doe'),
('../../../test/1__M_Right_little_finger.BMP', 'John Doe'),
('../../../test/1__M_Right_middle_finger.BMP', 'John Doe'),
('../../../test/1__M_Right_ring_finger.BMP', 'John Doe'),
('../../../test/1__M_Right_thumb_finger.BMP', 'John Doe'),
('../../../test/2__F_Left_index_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Left_middle_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Left_ring_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Left_thumb_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Right_index_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Right_little_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Right_middle_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Right_ring_finger.BMP', 'Jane Smith'),
('../../../test/2__F_Right_thumb_finger.BMP', 'Jane Smith'),
('../../../test/3__M_Left_index_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Left_little_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Left_middle_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Left_ring_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Left_thumb_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Right_index_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Right_little_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Right_middle_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Right_ring_finger.BMP', 'Alice Johnson'),
('../../../test/3__M_Right_thumb_finger.BMP', 'Alice Johnson'),
('../../../test/4__M_Left_index_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Left_little_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Left_middle_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Left_ring_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Left_thumb_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Right_index_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Right_little_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Right_middle_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Right_ring_finger.BMP', 'Bob Brown'),
('../../../test/4__M_Right_thumb_finger.BMP', 'Bob Brown'),
('../../../test/5__M_Left_index_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Left_little_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Left_middle_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Left_ring_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Left_thumb_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Right_index_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Right_little_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Right_middle_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Right_ring_finger.BMP', 'Charlie Davis'),
('../../../test/5__M_Right_thumb_finger.BMP', 'Charlie Davis'),
('../../../test/6__M_Left_index_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Left_little_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Left_middle_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Left_ring_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Left_thumb_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Right_index_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Right_little_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Right_middle_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Right_ring_finger.BMP', 'Diana Miller'),
('../../../test/6__M_Right_thumb_finger.BMP', 'Diana Miller'),
('../../../test/7__M_Left_index_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Left_little_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Left_middle_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Left_ring_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Left_thumb_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Right_index_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Right_little_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Right_middle_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Right_ring_finger.BMP', 'Eve Wilson'),
('../../../test/7__M_Right_thumb_finger.BMP', 'Eve Wilson'),
('../../../test/8__M_Left_index_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Left_little_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Left_middle_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Left_ring_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Left_thumb_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Right_index_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Right_little_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Right_middle_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Right_ring_finger.BMP', 'Frank Moore'),
('../../../test/8__M_Right_thumb_finger.BMP', 'Frank Moore'),
('../../../test/9__M_Left_index_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Left_little_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Left_middle_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Left_ring_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Left_thumb_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Right_index_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Right_little_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Right_middle_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Right_ring_finger.BMP', 'Grace Lee'),
('../../../test/9__M_Right_thumb_finger.BMP', 'Grace Lee'),
('../../../test/10__M_Left_index_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Left_little_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Left_middle_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Left_ring_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Left_thumb_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Right_index_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Right_little_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Right_middle_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Right_ring_finger.BMP', 'Henry Martin'),
('../../../test/10__M_Right_thumb_finger.BMP', 'Henry Martin'),
('../../../test/11__M_Left_index_finger.BMP', 'Isabella White'),
('../../../test/11__M_Left_little_finger.BMP', 'Isabella White'),
('../../../test/11__M_Left_middle_finger.BMP', 'Isabella White'),
('../../../test/11__M_Left_ring_finger.BMP', 'Isabella White'),
('../../../test/11__M_Left_thumb_finger.BMP', 'Isabella White'),
('../../../test/11__M_Right_index_finger.BMP', 'Isabella White'),
('../../../test/11__M_Right_little_finger.BMP', 'Isabella White'),
('../../../test/11__M_Right_middle_finger.BMP', 'Isabella White'),
('../../../test/11__M_Right_ring_finger.BMP', 'Isabella White'),
('../../../test/11__M_Right_thumb_finger.BMP', 'Isabella White'),
('../../../test/12__M_Left_index_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Left_little_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Left_middle_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Left_ring_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Left_thumb_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Right_index_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Right_little_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Right_middle_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Right_ring_finger.BMP', 'Jack Anderson'),
('../../../test/12__M_Right_thumb_finger.BMP', 'Jack Anderson'),
('../../../test/13__F_Left_index_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Left_little_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Left_middle_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Left_ring_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Left_thumb_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Right_index_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Right_little_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Right_middle_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Right_ring_finger.BMP', 'Karen Martinez'),
('../../../test/13__F_Right_thumb_finger.BMP', 'Karen Martinez'),
('../../../test/14__M_Left_index_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Left_little_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Left_middle_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Left_ring_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Left_thumb_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Right_index_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Right_little_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Right_middle_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Right_ring_finger.BMP', 'Liam Taylor'),
('../../../test/14__M_Right_thumb_finger.BMP', 'Liam Taylor'),
('../../../test/15__F_Left_index_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Left_little_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Left_middle_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Left_ring_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Left_thumb_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Right_index_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Right_little_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Right_middle_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Right_ring_finger.BMP', 'Mia Wilson'),
('../../../test/15__F_Right_thumb_finger.BMP', 'Mia Wilson'),
('../../../test/16__M_Left_index_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Left_little_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Left_middle_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Left_ring_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Left_thumb_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Right_index_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Right_little_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Right_middle_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Right_ring_finger.BMP', 'Noah Thomas'),
('../../../test/16__M_Right_thumb_finger.BMP', 'Noah Thomas'),
('../../../test/17__M_Left_index_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Left_little_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Left_middle_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Left_ring_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Left_thumb_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Right_index_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Right_little_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Right_middle_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Right_ring_finger.BMP', 'Olivia Garcia'),
('../../../test/17__M_Right_thumb_finger.BMP', 'Olivia Garcia'),
('../../../test/18__M_Left_index_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Left_little_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Left_middle_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Left_ring_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Left_thumb_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Right_index_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Right_little_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Right_middle_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Right_ring_finger.BMP', 'Patrick Rodriguez'),
('../../../test/18__M_Right_thumb_finger.BMP', 'Patrick Rodriguez'),
('../../../test/19__M_Left_index_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Left_little_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Left_middle_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Left_ring_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Left_thumb_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Right_index_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Right_little_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Right_middle_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Right_ring_finger.BMP', 'Sophia Hernandez'),
('../../../test/19__M_Right_thumb_finger.BMP', 'Sophia Hernandez'),
('../../../test/20__M_Left_index_finger.BMP', 'William Lopez'),
('../../../test/20__M_Left_little_finger.BMP', 'William Lopez'),
('../../../test/20__M_Left_middle_finger.BMP', 'William Lopez'),
('../../../test/20__M_Left_ring_finger.BMP', 'William Lopez'),
('../../../test/20__M_Left_thumb_finger.BMP', 'William Lopez'),
('../../../test/20__M_Right_index_finger.BMP', 'William Lopez'),
('../../../test/20__M_Right_little_finger.BMP', 'William Lopez'),
('../../../test/20__M_Right_middle_finger.BMP', 'William Lopez'),
('../../../test/20__M_Right_ring_finger.BMP', 'William Lopez'),
('../../../test/20__M_Right_thumb_finger.BMP', 'William Lopez'),
('../../../test/21__M_Left_index_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Left_little_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Left_middle_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Left_ring_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Left_thumb_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Right_index_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Right_little_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Right_middle_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Right_ring_finger.BMP', 'Emma Baker'),
('../../../test/21__M_Right_thumb_finger.BMP', 'Emma Baker'),
('../../../test/22__M_Left_index_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Left_little_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Left_middle_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Left_ring_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Left_thumb_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Right_index_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Right_little_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Right_middle_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Right_ring_finger.BMP', 'Gabriel Clark'),
('../../../test/22__M_Right_thumb_finger.BMP', 'Gabriel Clark'),
('../../../test/23__M_Left_index_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Left_little_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Left_middle_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Left_ring_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Left_thumb_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Right_index_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Right_little_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Right_middle_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Right_ring_finger.BMP', 'Hannah Adams'),
('../../../test/23__M_Right_thumb_finger.BMP', 'Hannah Adams'),
('../../../test/24__F_Left_index_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Left_little_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Left_middle_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Left_ring_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Left_thumb_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Right_index_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Right_little_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Right_middle_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Right_ring_finger.BMP', 'Ian Roberts'),
('../../../test/24__F_Right_thumb_finger.BMP', 'Ian Roberts'),
('../../../test/25__F_Left_index_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Left_little_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Left_middle_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Left_ring_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Left_thumb_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Right_index_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Right_little_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Right_middle_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Right_ring_finger.BMP', 'Julia Cooper'),
('../../../test/25__F_Right_thumb_finger.BMP', 'Julia Cooper'),
('../../../test/26__M_Left_index_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Left_little_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Left_middle_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Left_ring_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Left_thumb_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Right_index_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Right_little_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Right_middle_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Right_ring_finger.BMP', 'Kevin Evans'),
('../../../test/26__M_Right_thumb_finger.BMP', 'Kevin Evans'),
('../../../test/27__M_Left_index_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Left_little_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Left_middle_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Left_ring_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Left_thumb_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Right_index_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Right_little_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Right_middle_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Right_ring_finger.BMP', 'Lily Bennett'),
('../../../test/27__M_Right_thumb_finger.BMP', 'Lily Bennett'),
('../../../test/28__M_Left_index_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Left_little_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Left_middle_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Left_ring_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Left_thumb_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Right_index_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Right_little_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Right_middle_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Right_ring_finger.BMP', 'Michael Gray'),
('../../../test/28__M_Right_thumb_finger.BMP', 'Michael Gray'),
('../../../test/29__F_Left_index_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Left_little_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Left_middle_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Left_ring_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Left_thumb_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Right_index_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Right_little_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Right_middle_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Right_ring_finger.BMP', 'Natalie Kelly'),
('../../../test/29__F_Right_thumb_finger.BMP', 'Natalie Kelly'),
('../../../test/30__F_Left_index_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Left_little_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Left_middle_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Left_ring_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Left_thumb_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Right_index_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Right_little_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Right_middle_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Right_ring_finger.BMP', 'Oscar Hill'),
('../../../test/30__F_Right_thumb_finger.BMP', 'Oscar Hill');


INSERT INTO `biodata` (`NIK`, `nama`, `tempat_lahir`, `tanggal_lahir`, `jenis_kelamin`, `golongan_darah`, `alamat`, `agama`, `status_perkawinan`, `pekerjaan`, `kewarganegaraan`) VALUES
('1234567890123451', 'Jhn D', 'Jakarta', '1980-01-01', 'Laki-Laki', 'O', 'Jl. Merdeka No.1', 'Islam', 'Menikah', 'Programmer', 'Indonesia'),
('1234567890123452', 'Jn Smth', 'Bandung', '1985-02-02', 'Perempuan', 'A', 'Jl. Kemerdekaan No.2', 'Kristen', 'Belum Menikah', 'Designer', 'Indonesia'),
('1234567890123453', 'Alc Jhnsn', 'Surabaya', '1990-03-03', 'Perempuan', 'B', 'Jl. Pahlawan No.3', 'Hindu', 'Menikah', 'Doctor', 'Indonesia'),
('1234567890123454', 'Bb Brwn', 'Medan', '1975-04-04', 'Laki-Laki', 'AB', 'Jl. Sejahtera No.4', 'Buddha', 'Cerai', 'Engineer', 'Indonesia'),
('1234567890123455', 'Chrls Dvs', 'Yogyakarta', '1995-05-05', 'Laki-Laki', 'O', 'Jl. Keadilan No.5', 'Islam', 'Belum Menikah', 'Teacher', 'Indonesia'),
('1234567890123456', 'Dn Mllr', 'Bali', '1988-06-06', 'Perempuan', 'A', 'Jl. Surga No.6', 'Kristen', 'Menikah', 'Nurse', 'Indonesia'),
('1234567890123457', 'Ev Wlsn', 'Makassar', '1993-07-07', 'Perempuan', 'B', 'Jl. Damai No.7', 'Islam', 'Belum Menikah', 'Lawyer', 'Indonesia'),
('1234567890123458', 'Frnk Mr', 'Semarang', '1982-08-08', 'Laki-Laki', 'AB', 'Jl. Harmoni No.8', 'Buddha', 'Menikah', 'Architect', 'Indonesia'),
('1234567890123459', 'Grc L', 'Palembang', '1991-09-09', 'Perempuan', 'O', 'Jl. Kebahagiaan No.9', 'Hindu', 'Belum Menikah', 'Scientist', 'Indonesia'),
('1234567890123460', 'Hn Mrt', 'Balikpapan', '1978-10-10', 'Laki-Laki', 'A', 'Jl. Kesatuan No.10', 'Islam', 'Cerai', 'Entrepreneur', 'Indonesia'),
('1234567890123461', 'Isbll W', 'Surakarta', '1984-11-11', 'Perempuan', 'AB', 'Jl. Bahagia No.11', 'Kristen', 'Belum Menikah', 'Writer', 'Indonesia'),
('1234567890123462', 'Jck Adr', 'Denpasar', '1979-12-12', 'Laki-Laki', 'O', 'Jl. Kebanggaan No.12', 'Islam', 'Menikah', 'Artist', 'Indonesia'),
('1234567890123463', 'Krn Mrtz', 'Bandar Lampung', '1992-01-13', 'Perempuan', 'B', 'Jl. Cinta No.13', 'Kristen', 'Belum Menikah', 'Chef', 'Indonesia'),
('1234567890123464', 'Lm Tylr', 'Pontianak', '1987-02-14', 'Laki-Laki', 'A', 'Jl. Hati No.14', 'Buddha', 'Cerai', 'Musician', 'Indonesia'),
('1234567890123465', 'Ma Wlsn', 'Manado', '1993-03-15', 'Perempuan', 'AB', 'Jl. Kasih No.15', 'Islam', 'Menikah', 'Dancer', 'Indonesia'),
('1234567890123466', 'Nah Thms', 'Padang', '1980-04-16', 'Laki-Laki', 'O', 'Jl. Rindu No.16', 'Kristen', 'Belum Menikah', 'Actor', 'Indonesia'),
('1234567890123467', 'Olv Grc', 'Pekanbaru', '1995-05-17', 'Perempuan', 'B', 'Jl. Bahagia No.17', 'Islam', 'Belum Menikah', 'Singer', 'Indonesia'),
('1234567890123468', 'Ptrck Rdrgz', 'Jayapura', '1988-06-18', 'Laki-Laki', 'A', 'Jl. Damai No.18', 'Buddha', 'Menikah', 'Athlete', 'Indonesia'),
('1234567890123469', 'Sph Hrnndz', 'Aceh', '1994-07-19', 'Perempuan', 'AB', 'Jl. Senang No.19', 'Islam', 'Cerai', 'Politician', 'Indonesia'),
('1234567890123470', 'Wlm Lpz', 'Samarinda', '1982-08-20', 'Laki-Laki', 'O', 'Jl. Bahagia No.20', 'Kristen', 'Belum Menikah', 'Journalist', 'Indonesia'),
('1234567890123471', 'Emm Bkr', 'Bandung', '1986-09-21', 'Perempuan', 'B', 'Jl. Bahagia No.21', 'Kristen', 'Menikah', 'Writer', 'Indonesia'),
('1234567890123472', 'Gbrl Clrk', 'Surabaya', '1991-10-22', 'Laki-Laki', 'AB', 'Jl. Sukacita No.22', 'Islam', 'Belum Menikah', 'Artist', 'Indonesia'),
('1234567890123473', 'Hnnh Adms', 'Jakarta', '1987-11-23', 'Perempuan', 'O', 'Jl. Sukma No.23', 'Hindu', 'Cerai', 'Chef', 'Indonesia'),
('1234567890123474', 'In Rbrts', 'Semarang', '1992-12-24', 'Laki-Laki', 'A', 'Jl. Cinta No.24', 'Kristen', 'Belum Menikah', 'Musician', 'Indonesia'),
('1234567890123475', 'Jla Cpr', 'Bandar Lampung', '1988-01-25', 'Perempuan', 'AB', 'Jl. Harapan No.25', 'Buddha', 'Menikah', 'Dancer', 'Indonesia'),
('1234567890123476', 'Kvn Evns', 'Yogyakarta', '1993-02-26', 'Laki-Laki', 'O', 'Jl. Damai No.26', 'Islam', 'Belum Menikah', 'Actor', 'Indonesia'),
('1234567890123477', 'Lly Bnntt', 'Denpasar', '1989-03-27', 'Perempuan', 'B', 'Jl. Indah No.27', 'Kristen', 'Cerai', 'Singer', 'Indonesia'),
('1234567890123478', 'Mchl Gry', 'Makassar', '1994-04-28', 'Laki-Laki', 'A', 'Jl. Raya No.28', 'Islam', 'Menikah', 'Athlete', 'Indonesia'),
('1234567890123479', 'Ntl Kll', 'Medan', '1990-05-29', 'Perempuan', 'O', 'Jl. Permai No.29', 'Kristen', 'Belum Menikah', 'Politician', 'Indonesia'),
('1234567890123480', 'Oscr Hll', 'Pontianak', '1985-06-30', 'Laki-Laki', 'AB', 'Jl. Bahagia No.30', 'Islam', 'Menikah', 'Journalist', 'Indonesia');
