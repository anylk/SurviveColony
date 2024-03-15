using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKController : MonoBehaviour
{
    public RigBuilder rigBuilder;
    [Header("RigLayer Weapon Pose")]
    public Rig rigLayerWeaponPose;
    public MultiPositionConstraint weaponPoseMultiPositionConstraint;
    public MultiParentConstraint weaponPoseMultiParentConstraint;
    [Header("RigLayer Weapon Aim")]
    public Rig rigLayerWeaponPoseAim;
    public MultiPositionConstraint weponPoseAimMultiPositionConstraint;
    public MultiParentConstraint weaponPoseAimMultiParentConstraint;
    [Header("RigLayer Hand")]
    public Rig rigLayerHand;
    public TwoBoneIKConstraint rightHand;
    public TwoBoneIKConstraint leftHand;
    [Header("Body Aim")]
    public Rig rigLayerBodyAim;

    public void SetWeaponData(IKWeaponData _ikWeaponData,Transform weaponObject)
    {
        //Right Hand
        int rightHandWeight = _ikWeaponData.rightHandGrip == null ? 0 : 1;
        rightHand.weight = rightHandWeight;
        rightHand.data.target = _ikWeaponData.rightHandGrip;
        //Left Hand
        int leftHandWeight = _ikWeaponData.leftHandGrip == null ? 0 : 1;
        leftHand.weight = leftHandWeight;
        leftHand.data.target = _ikWeaponData.leftHandGrip;
        //Weapon Aim Pose
        weaponPoseAimMultiParentConstraint.data.constrainedObject = weaponObject;
        weponPoseAimMultiPositionConstraint.data.offset = _ikWeaponData.aimingMultiPositionConstraintOffset;
        weponPoseAimMultiPositionConstraint.transform.localRotation = _ikWeaponData.aimingTransformRotation;
        //Weapon Pose
        weaponPoseMultiParentConstraint.data.constrainedObject = weaponObject;
        weaponPoseMultiPositionConstraint.data.offset = _ikWeaponData.idleMultiPositionConstraintOffset;
        weaponPoseMultiPositionConstraint.transform.localRotation = _ikWeaponData.idleTransformRotation;
        //Build
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
    [Space(4)]
    [Header("Aim")]
    public Vector3 aimingMultiPositionConstraintOffset;
    public Quaternion aimingTransformRotation;
    [Space(4)]
    [Header("Idle")]
    public Vector3 idleMultiPositionConstraintOffset;
    public Quaternion idleTransformRotation;
}