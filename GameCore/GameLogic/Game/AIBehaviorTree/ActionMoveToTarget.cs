﻿using System;
using System.Collections.Generic;
using BehaviorTree;
using GameLogic.Game.Elements;
using Layout.AITree;

namespace GameLogic.Game.AIBehaviorTree
{
	[TreeNodeParse(typeof(TreeNodeMoveToTarget))]
	public class ActionMoveToTarget:ActionComposite,ITreeNodeHandle
	{
		public ActionMoveToTarget()
		{
		}

		public override IEnumerable<RunStatus> Execute(ITreeRoot context)
		{
			var root = context as AITreeRoot;
			var index = root["TargetIndex"];
			if (index == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			var target = root.Perception.State[(long)index] as BattleCharacter;
			if (target == null)
			{
				yield return RunStatus.Failure;
				yield break;
			}

			//float lastTime = root.Time-2;
			var pos = target.View.Transform.Position;
			root.Character.View.MoveTo(pos);
			view = root.Character.View;
			while (root.Perception.Distance(target, root.Character) > stopDistance)
			{
				if (root.Perception.View.Distance(pos, target.View.Transform.Position) > 0.2f)
				{
					root.Character.View.MoveTo(target.View.Transform.Position);
					pos = target.View.Transform.Position;
				}
				yield return RunStatus.Running;
			}

			root.Character.View.StopMove();

			yield return RunStatus.Success;

		}

		public void SetTreeNode(TreeNode node)
		{
			var n = node as TreeNodeMoveToTarget;
			stopDistance = n.distance;
		}

		private IBattleCharacter view;

		private float stopDistance = 0f;

		public override void Stop(ITreeRoot context)
		{
			
			if (LastStatus.HasValue && LastStatus.Value == RunStatus.Running && view != null)
				view.StopMove();
			base.Stop(context);
		}
	}
}
