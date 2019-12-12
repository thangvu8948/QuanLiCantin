go
use master
go
if db_id('qlct') is not null
	drop database qlct
go
create Database qlct;
go
use qlct;

create table category
(
MALOAI char(5) not null,
TENLOAI nchar(50),
constraint PK_Cat
primary key(MALOAI)
);

create table MonAn
(
MAMON char(5) not null,
TENMON nchar(50),
GIATIEN int,
SOLUONG int,
MALOAI char(5),
HinhAnh varchar(50)
constraint PK_MonAn
primary key(MAMON)
);
create table NhanVien
(
MaNV char(5) not null,
LoaiNV int not null,
Ten nchar(50),
TenDN char(30) not null,
MatKhau char(30) not null,
constraint PK_NhanVien
primary key(MaNV)
);

create table HoaDon
(
MaHD char(5) not null,
NgayLap date,
ThanhTien float,
constraint PK_HD
primary key(MaHD)
);
create table ChiTiet
(
MaHD char(5) not null,
MaSP char(5) not null,
SoLuong int,
ThanhTien float,
constraint PK_ChiTiet
primary key(MaHD,MaSP)
);
create table LoaiTaiKhoan
(
LoaiTK int not null,
TenLoai nchar(30),
constraint PK_LTK
primary key(LoaiTK)
);
create table HangHoa
(
MaHH char(5) not null,
TenHH nchar(50),
SoLuong float,
constraint PK_HH
primary key(MaHH)
);
create table Kho
(
MaLuuKho char(6) not null,
MaHH char(5) not null,
SoLuongDauNgay float,
SoLuongCuoiNgay float,
NgayLuuKho date,
constraint PK_K
primary key(MaLuuKho)
);
create table Nhap
(
MaNhap char(5) not null,
MaHH char(5) not null,
NgayNhap date,
SoLuong float,
DonGia float,
ThanhTien float,
constraint PK_N
primary key(MaNhap)
);
alter table NhanVien
add
	constraint FK_NV_LTK
    foreign key(LoaiNV)
    references LoaiTaiKhoan(LoaiTK);
alter table MonAn
add
	constraint FK_TD_ML
	foreign key(MALOAI)
	references category(MALOAI);
alter table ChiTiet
add 
	constraint FK_CT_TD
    foreign key(MaSP)
    references MonAn(MAMON);
alter table ChiTiet
add
	constraint FK_CT_HD
    foreign key(MaHD)
    references HoaDon(MaHD);
alter table Kho
add
	constraint FK_K_HH
    foreign key(MaHH)
    references HangHoa(MaHH);
alter table Nhap
add
	constraint FK_N_HH
    foreign key(MaHH)
    references HangHoa(MaHH);
insert category
values ('1', N'Điểm tâm'),
		('2', N'Ăn trưa'),
		('3', N'Nước uống, tráng miệng');

insert MonAn
values ('1', N'Bánh mì ốp la', 15000, 100, '1','Images/1/download.jpg'),
		('2', N'Cơm tấm sườn bì', 20000, 100, '1','Images/2/download.jpg'),
		('3', N'Bánh canh', 25000, 100, '1', 'Images/3/download.jpg'),
		('4', N'Hủ tiếu mì', 25000, 100,'1', 'Images/4/download.jpg'),
		('5', N'Bún gạo', 25000, 100, '1', 'Images/5/download.jpg'),
		('6', N'Spaghetti', 25000, 100,'1', 'Images/6/download.jpg'),
		('7', N'Mì gói thịt bò', 30000, 100, '1', 'Images/7/download.jpg'),
		('8', N'Bún bò Huế', 30000, 100, '1', 'Images/8/download.jpg'),
		('9', N'Cơm phần', 20000, 100, '2', 'Images/9/download.jpg'),
		('10',N'Cơm dĩa đặc biệt', 30000, 100, '2', 'Images/10/download.jpg'),
		('11',N'Cơm dĩa', 25000, 100, '2', 'Images/11/download.jpg') ,
		('12',N'Trà chanh đường',6000, 100,'3', 'Images/12/download.jpg'),
		('13',N'Rau má đậu xanh',6000, 100,'3', 'Images/13/download.jpg'),
		('14',N'Sữa đậu xanh',6000, 100,'3', 'Images/14/download.jpg'),
		('15',N'Đậu nành',6000, 100,'3', 'Images/15/download.jpg'),
		('16',N'Nước đá me',6000, 100,'3', 'Images/16/download.jpg'),
		('17',N'Nước tắc ép',6000, 100,'3', 'Images/17/download.jpg'),
		('18',N'Chanh dây',6000, 100,'3', 'Images/18/download.jpg'),
		('19',N'Chanh muối',6000, 100,'3', 'Images/19/download.jpg'),
		('20',N'Sâm dứa',7000, 100,'3', 'Images/20/download.jpg'),
		('21',N'Cocktail',8000, 100,'3', 'Images/21/download.jpg'),
		('22',N'Sữa nóng',8000, 100,'3', 'Images/22/download.jpg'),
		('23',N'Sâm bổ lượng',8000 , 100,'3', 'Images/23/download.jpg'),
		('24',N'Sữa chua',8000, 100,'3', 'Images/24/download.jpg'),
		('25',N'Cam vắt',15000, 100,'3', 'Images/25/download.jpg');
insert LoaiTaiKhoan
values (1, 'admin'),
		(2, 'staff');
insert NhanVien
values ('NV001','1',N'Lê Văn An','admin','admin'),
		('NV002','2',N'Nguyễn Thị Vân Anh','staff1','staff'),
        ('NV003','2',N'Đặng Văn Cường','staff2','staff'),
        ('NV004','2',N'Lê Thị Thúy Hằng','staff3','staff');
insert HoaDon
values ('HD001','2019-11-15',21000),
		('HD002','2019-11-15',31000),
        ('HD003','2019-11-16',18000),
        ('HD004','2019-11-17',45000);
insert ChiTiet
values ('HD001','1',1,15000),
		('HD001','12',1,6000),
        ('HD002','11',1,25000),
        ('HD002','13',1,6000),
        ('HD003','16',1,6000),
        ('HD003','18',1,6000),
        ('HD003','19',1,6000),
        ('HD004','7',1,30000),
        ('HD004','25',1,15000);
insert HangHoa
values ('HH001',N'Gạo',100),
		('HH002',N'Thịt ba chỉ',10),
        ('HH003',N'Thịt bò',5.5),
        ('HH004',N'Rau cải',15),
        ('HH005',N'Mì kí',20),
        ('HH006',N'Đường',15),
        ('HH007',N'Bột ngọt',10);
insert Kho
values ('LK0001','HH001',100,91,'2019-11-15'),
		('LK0002','HH002',10,8,'2019-11-15'),
        ('LK0003','HH003',5,4,'2019-11-15'),
        ('LK0004','HH004',15,13.3,'2019-11-15'),
        ('LK0005','HH005',20,10.5,'2019-11-15'),
        ('LK0006','HH006',15,12.5,'2019-11-15'),
        ('LK0007','HH007',10,9,'2019-11-15'),
        ('LK0008','HH001',121,80,'2019-11-16');
insert Nhap
values ('N0001','HH001','2019-11-16',30,10000,300000),
		('N0002','HH003','2019-11-16',2.5,240000,600000),
        ('N0003','HH003','2019-11-16',1.5,240000,360000),
        ('N0004','HH006','2019-11-16',5,25000,125000),
        ('N0005','HH005','2019-11-17',20,30000,600000),
        ('N0006','HH002','2019-11-17',10,160000,1600000);

		select * from Kho
		select * from HangHoa
		select * from Nhap
		select * from NhanVien