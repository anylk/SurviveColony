using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKController : MonoBehaviour
{
    public RigBuilder rigBuilder;
    public Rig rigLayerHand;
    public TwoBoneIKConstraint rightHand;
    public TwoBoneIKConstraint leftHand;
    public Rig rigLayerWeaponPoseIdle;
    public Rig rigLayerWeaponPoseAim;
    public Rig rigLayerBodyAim;
    public MultiPositionConstraint weaponPoseIdle;
    public MultiPositionConstraint weaponPoseAim;

    public void SetWeaponData(IKWeaponData _ikWeaponData)
    {
        int rightHandWeight = _ikWeaponData.rightHandGrip == null ? 0 : 1;
        rightHand.data.target = _ikWeaponData.rightHandGrip;
        rightHand.weight = rightHandWeight;
        int leftHandWeight = _ikWeaponData.leftHandGrip == null ? 0 : 1;
        leftHand.data.target = _ikWeaponData.leftHandGrip;
        leftHand.weight = leftHandWeight;
        weaponPoseIdle.data.offset = _ikWeaponData.idleMultiPositionConstraintOffset;
        weaponPoseAim.data.offset = _ikWeaponData.aimingMultiPositionConstraintOffset;
        weaponPoseAim.transform.localRotation = _ikWeaponData.aimingTransformRotation;
        weaponPoseIdle.transform.localRotation = _ikWeaponData.idleTransformRotation;
        rigBuilder.Build();
    }

    public void UpdateAim(float _weight)
    {
        rigLayerWeaponPoseAim.weight = _weight;
        rigLayerBodyAim.weight = _weight;
    }
}
[System.Serializable]
public struct IKWeaponData
{
    public Transform rightHandGrip;
    public Transform leftHandGrip;

    public Vector3 aimingMultiPositionConstraintOffset;
    public Quaternion aimingTransformRotation;
    public Vector3 idleMultiPositionConstraintOffset;
    public Quaternion idleTransformRotation;
}