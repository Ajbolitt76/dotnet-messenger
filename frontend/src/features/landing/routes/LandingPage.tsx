import { useAuth } from "@/lib/AuthProvider"
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

export const LandingPage: React.FC = () => {
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => { navigate("/chats") });

  return (<></>)
}