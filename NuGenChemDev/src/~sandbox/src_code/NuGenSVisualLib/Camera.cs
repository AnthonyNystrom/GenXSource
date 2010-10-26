using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.UI
{
	public class Camera
	{
		public Vector3 Position;
		private Vector3 Target;
		private Vector3 Up;
        private Device device;
		public const float degToRad = 0.0174532925f;
		public const float radToDeg = 57.2957795f;
		public float ForwardVelocity = 0.0f;
		public float StrafeVelocity = 0.0f;

        public Camera(Device dev, Vector3 pos, Vector3 target)
		{
			device = dev;
			Position = pos;
			Target = target;
			Up = new Vector3(0, 1, 0);			
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			//Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}

		public void Update()
		{
			if( ForwardVelocity != 0.0f )
				MoveForward(-ForwardVelocity);
			if( StrafeVelocity != 0.0f )
				MoveSideways(-StrafeVelocity);
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
		}

		public void SetPosition(Vector3 pos, Vector3 target)
		{
			Position = pos;
			Target = target;
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
		}

		public void Yaw(float fDegrees)
		{
			float yaw = fDegrees * degToRad;
			Matrix mRot = Matrix.RotationY(yaw);
			Vector3 vTemp = Target - Position;
			vTemp.Normalize();
			vTemp.TransformCoordinate(mRot);
			vTemp *= 5.0f;
			Target = Position + vTemp;

			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			//Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}

		public void Pitch(float fDegrees)
		{
			float pitch = fDegrees * degToRad;
			Vector3 vTemp = Target - Position;
			if( vTemp.Z < 0.0f )
				pitch = -pitch;
			Matrix mRot = Matrix.RotationX(pitch);
			
			
			vTemp.Normalize();
			vTemp.TransformCoordinate(mRot);
			vTemp *= 5.0f;
			Target = Position + vTemp;

			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			//Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}

		public void Roll(float fDegrees)
		{
			float roll = fDegrees * degToRad;
			Matrix mRot = Matrix.RotationYawPitchRoll(0, 0, roll);
			Up.TransformCoordinate(mRot);
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}

		public void OrbitLeft(float fDegrees, Vector3 Anchor, float fDistance)
		{
			float fNewYaw = (fDegrees * degToRad);
			OrbitY(fNewYaw, Anchor, fDistance);
		}

		public void OrbitRight(float fDegrees, Vector3 Anchor, float fDistance)
		{
			float fNewYaw = (fDegrees * degToRad);
			OrbitY(-fNewYaw, Anchor, fDistance);
		}

		private void OrbitY(float newYaw, Vector3 Anchor, float fDistance)
		{
			Matrix mRot = Matrix.RotationY(-newYaw);
			Vector3 vTemp = Position - Anchor;
			vTemp.Normalize();
			vTemp *= fDistance;
			vTemp.TransformCoordinate(mRot);
			Position = Anchor + vTemp;
			Target = Anchor;
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			//Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}

		public void MoveForward(float fUnits)
		{
			Vector3 vTemp = Position - Target;
			vTemp.Normalize();
			vTemp *= fUnits;
			Position += vTemp;
			Target += vTemp;
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			//Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}

		public void MoveSideways(float fUnits)
		{
			Vector3 vDir = Position - Target;
			Vector3 vSide = Vector3.Cross(vDir, Up);
			vSide.Normalize();
			vSide *= -fUnits;
			Position += vSide;
			Target += vSide;
			device.Transform.View = Matrix.LookAtLH(Position, Target, Up);
			//Up = new Vector3(device.Transform.View.M12, device.Transform.View.M22, device.Transform.View.M32);
		}
	}
}
