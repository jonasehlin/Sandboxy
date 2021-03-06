// TestBCrypt.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <bcrypt.h>
#include "TestBCrypt.h"

#define NT_SUCCESS(Status)  (((NTSTATUS)(Status)) >= 0)

#define STATUS_UNSUCCESSFUL ((NTSTATUS)0xC0000001L)

int main()
{
	LPCSTR value = "Apanson ar ett miffo";
	ULONG valueLength = strlen(value);

	BCRYPT_ALG_HANDLE hAlgorithm;

	NTSTATUS status = BCryptOpenAlgorithmProvider(
		&hAlgorithm,
		BCRYPT_SHA256_ALGORITHM,
		MS_PRIMITIVE_PROVIDER,
		0
	);
	if (!NT_SUCCESS(status))
		return -1;

	UCHAR buffer[256];
	ULONG bufferLength;
	status = BCryptGetProperty(
		hAlgorithm,
		BCRYPT_HASH_LENGTH,
		buffer,
		sizeof(buffer),
		&bufferLength,
		0
	);
	if (!NT_SUCCESS(status))
		return -1;

	UCHAR output[32];
	status = BCryptHash(
		hAlgorithm,
		NULL,
		0,
		(PUCHAR)value,
		valueLength,
		output,
		sizeof(output)
	);
	if (!NT_SUCCESS(status))
		return -1;



	status = BCryptCloseAlgorithmProvider(hAlgorithm, 0);
	if (!NT_SUCCESS(status))
		return -1;

    return 0;
}

