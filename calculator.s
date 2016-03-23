# Calculator
			.data           ## Constant declaration
# word
buffer:		.space 100					# input reserve 100
newline:	.word '\n'
overflow:	.asciiz "overflow"

.text        		   			## Code section

read_int:
	li		$t3, 0				# temp = 0
ri_while:
	blt		$t1, '0', ri_end	# check input = 0-9
	bgt		$t1, '9', ri_end
	subu	$t1, $t1, '0'		# convert char to int
	mul		$t3, $t3, 10		# temp = temp * 10
	add		$t3, $t3, $t1		# + char
	addiu	$t0, $t0, 1			# index++
	lb		$t1, ($t0)
	bne		$t1, $0, ri_while	# not end of string
ri_end:
	move	$t2, $t3			# return temp
	jr		$ra

cal:
	sub		$sp, 12				# stack pointer
	sw		$s0, 0($sp)			# save arg = prev
	move	$s0, $a0
	sw		$ra, 4($sp)			# save ra
	sw		$t2, 8($sp)			# save B
	move	$t4, $t1
	addiu	$t0, $t0, 1			# index++
	lb		$t1, ($t0)
	jal		read_int			# B = read_int

	beq		$t4, '+', cal_add	# if('+')
	beq		$t4, '-', cal_sub	# if('-')
	beq		$t4, '*', cal_mul	# if('*')
	beq		$t4, '/', cal_div	# if('/')
	move	$v0, $a0			# else return prev
	j		cal_end
cal_add:
	move	$a0, $t2			# arg = B
	jal		cal					# cal(B)
	move	$t2, $v0			# B = cal(B)
	addu	$v0, $s0, $t2		# return = prev + cal(B)

	add_ovf:
		and		$s1, $s0, 0x80000000	# s1 = sign A
		and		$s2, $t2, 0x80000000	# s2 = sign B
		xor		$s3, $s1, $s2			# s3 = (sign A != sign B)
		bne		$s3, 0, cal_end			# sign A != sign B > not OVF
		and		$s2, $v0, 0x80000000	# s2 = sign sum
		xor		$s3, $s1, $s2			# sign A != sign sum
		beq		$s3, 0, cal_end			# sign A == sign sum > not OVF
		li		$t5, 1					# else ovf = true

		j		cal_end
cal_sub:
	mul		$t2, $t2, -1		# temp*-1
	move	$a0, $t2
	jal		cal					# cal(temp*-1)
	move	$t2, $v0
	addu	$v0, $s0, $t2		# return = prev + cal(temp*-1)
	j		add_ovf
cal_mul:
	multu	$t2, $s0			# temp*prev
	mflo	$a0
	mfhi	$s1					# s1 = hi
	beq		$s1, 0, mul_next	# hi == 0 > next
	li		$t5, 1				# ovf = true

	mul_next:
		jal		cal					# cal(temp*prev)
		j		cal_end
cal_div:
	div		$s0, $t2			# temp/prev
	mflo	$a0
	bne		$t2, 0, div_next	# not div 0 > next
	li		$t5, 1				# ovf = true

	div_next:
		jal		cal					# cal(temp*prev)
		j		cal_end

cal_end:
	lw		$s0, 0($sp)			# restore register
	lw		$ra, 4($sp)
	lw		$t2, 8($sp)
	add		$sp, 12
	jr		$ra

main:
	li		$t5, 0				# ovf = false

	li		$v0, 8				# input = load
	la		$a0, buffer
	li		$a1, 100
	syscall
	move	$t0, $a0
	lb		$t1, ($t0)			# index point

	sub		$sp, 4				# save register
	sw		$ra, 0($sp)

	jal		read_int
	move	$a0, $t2			# a0 = readInt
	jal		cal					# ca(a0)
	move	$a0, $v0			# save value
	li		$v0, 1				# print
	syscall

	li		$v0, 4				# print \n
	la		$a0, enter
	syscall

	bne		$t5, 1, end			# not ovf > end
	li		$v0, 4				# print oveflow
	la		$a0, overflow
	syscall

end:
	lw		$ra, 0($sp)
	add		$sp, 4
	jr		$ra					# END
