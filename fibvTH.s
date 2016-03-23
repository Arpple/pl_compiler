.ข้อมูล
	พร้อม: .แอสกี้ "Input the number: "
	เส้นใหม่: .แอสกี้ "\n"
	ปัจจุบัน: .แอสกี้ "curren number: "
	goToM1: .แอสกี้ "Go to f(n-1) > "
	goToM2: .แอสกี้ "Go to f(n-2) > "
	ผลลัพธ์: .แอสกี้ "  Output : "
	กลับ: .แอสกี้ "     RETURN!!!!  \n"
	แมลง: .แอสกี้ "Why not jr !?"
.ข้อความ

Main:
	ลิ $v0,4
	ละ $a0,พร้อม
	ซิสคอล

	#read input
	ลิ $v0,5
	ซิสคอล

	#go to function
	หมูบ		$a0,$v0
	พุ่งเสียบ		Fibonanciiz

	#when end of process
	ลิ $v0,4
	ละ $a0,ผลลัพธ์
	ซิสคอล

	# print int
		หมูบ $a0,$s0
		ลิ $v0,1
		ซิสคอล
		ลิ $v0,10
		ซิสคอล
	#a0 has number for find fibonancii

	#fibo code is
	# int fibo(int n)
	# 	if n==0 return 1
	# 	if n==1 return 1
	# 	else return f(n-1)+f(n-2)

	Fibonanciiz:
		บวกอิ $sp,$sp,3 # use 2 parameter is [4=current n position][4=memory or last doing]
		เสพ $ra,0($sp) #store pointer
		เสพ $a0,1($sp) #store data (n)
		#save something,2($sp) #store data at f(n-1)

		โหลด $a0,1($sp) #cause i use a0 for check print:\
		ลิ $s0,1  #set default return 1
		#if f(n) <= 1 return 1 (s0)
			แยกน้อยอีคั่ว $a0,$0,RETURNF0 #case f0
			ลิ	$t7,1
			แยกน้อยอีคั่ว $a0,$t7,RETURNF1 #case f1
		#else
			โหลด $a0,1($sp) #cause i use a0 for check print:\
			# F(N-1)
			บวกอิ $a0,$a0,-1 # and go to f(n-1)
			พุ่งเสียบ Fibonanciiz # jump to fibo n-1
			เสพ $s0,2($sp) #store f(n-1) in stack cause when go f(n-2) data s1 has been change

			โหลด $a0,1($sp) #cause i use a0 for check print:\
			#F(N-2)
			โหลด $a0,1($sp) #load original data
			บวกอิ $a0,$a0,-2 # set f(n-2)
			พุ่งเสียบ Fibonanciiz # jump to fibo n-2
			หมูบ $s2,$s0
			โหลด $s1,2($sp) #load f(n-1) values to s1 for calculatate

			#RETURN STATEMENT
			บวก $t0,$s1,$s2
			หมูบ $s0,$t0

	END_Fibonanciiz:
		โหลด $ra,0($sp) #load before last position
		บวกอิ $sp,$sp,-3 #resotre pointer back -3 byte (3array)
		พุ่งร์ $ra

	RETURNF0:
		ลิ $s0,0 #set f0
		พุ่ง END_Fibonanciiz
	RETURNF1:
		ลิ $s0,1 #set f1
		พุ่ง END_Fibonanciiz
end:
