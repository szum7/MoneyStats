// This file is out of date! (2020-06-08)


/// User creates rules:



/// Rules
/// -----
/// (a & b & c) || (d & e) || f
/// Does the currently examined transaction meet the conditions?
/// 
/// Types:
/// - True rule (apply to every transaction)
/// - Has value of property
/// - Property's value contians a string
/// - Value of property is not null
/// - Value of property is null
/// 
/// 
/// Actions
/// -------
/// a1 & a2 & a3 & ...
/// Do a1&a2&a3&... actions on the evaluated transaction.
/// 
/// Types:
/// - Omit
/// - Add tags
/// - Set value of property
/// - Aggregate to a single transaction (additional actions apply to this transaction!)


// Example:
((Sum == -11000 && Message == null) || PartnerName == "Generali")
hasValue;"Sum";-11000;&&isPropertyNull;"Message";||hasValue;"PartnerName";"Generali"; 
=> set title to "Generali, havi díj"
=> add tags "monthly", "generali"

{
	title: "ruleset1",
	ruleGroups: 
	[
		{
			title: "rulegroup1",
			// rules array is a matrix. Columns are AND rules, rows are OR
			// [ 
			//	[a, b, c],
			//  [d],
			//  [e, f]
			// ]
			// => (a && b && c) || (d) || (e && f)
			rules: 
			[
				[{
					title: "rule1",
					type: "hasValue",
					property: "Sum",
					value: -11000
				}, {
					title: "rule2",
					type: "isPropertyNull",
					property: "Message",
					value: null
				}],
				[{
					title: "rule3",
					type: "hasValue",
					property: "PartnerName",
					value: "Generali"
				}]
			],
			actions: 
			[
				{
					title: "action1",
					type: "setProperty",
					property: "Title",
					value: "Generali, havi díj"
				},
				{
					title: "action2",
					type: "addTags",
					property: null,
					value: null,
					tags: [13, 16] // foreign key
				}				
			]
		}
	]
}