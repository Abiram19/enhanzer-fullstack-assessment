// Shape sent to the backend proxy: POST /api/auth/login
export interface LoginRequest {
  email: string;
  password: string;
}

export interface LocationDto {
  Location_Code?: string;
  Location_Name?: string;
  location_Code?: string;
  location_Name?: string;
}

// Shape returned by the backend proxy (maps to ClientLoginResponseDto)
export interface LoginResponse {
  success: boolean;
  statusCode: number;
  message: string | null;
  companyCode?: string | null;
}
