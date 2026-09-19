export interface LoginRequestBody {
  Username: string;
  Pw: string;
}

export interface LoginRequest {
  API_Action: string;
  Device_Id: string;
  Sync_Time: string;
  Company_Code: string;
  API_Body: LoginRequestBody[];
}

export interface LocationDto {
  Location_Code?: string;
  Location_Name?: string;
  location_Code?: string;
  location_Name?: string;
}

export interface LoginResponse {
  Status_Code: number;
  Sync_Time: string | null;
  Message: string | null;
  User_Locations?: LocationDto[];
  Response_Body: any;
}

